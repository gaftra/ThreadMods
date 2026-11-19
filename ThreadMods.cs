using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using ExileCore;
using Hexa.NET.ImGui;
using Color = SharpDX.Color;

namespace ThreadMods;

public sealed class ThreadMods : BaseSettingsPlugin<ThreadModsSettings>
{
    private const int MinimumAreaLevel = 68;
    private static readonly Color HeadingColor = new(230, 195, 120, 255);
    private static readonly ModDisplayMode[] DisplayModes =
        [ModDisplayMode.Highlight, ModDisplayMode.Normal, ModDisplayMode.Hide];

    private sealed record Row(string Name, int Value, bool Visible, int? VisibleValue);
    private sealed record Snapshot(uint Hash, string Area, Row[] Rows, string Error);
    private readonly Stopwatch _refresh = new();
    private Snapshot _snapshot;
    private string _search = "";

    public override void DrawSettings()
    {
        base.DrawSettings();
        if (!ImGui.TreeNode("Mods")) return;

        foreach (var type in Enum.GetValues<ThreadType>())
        {
            if (!ImGui.TreeNode(type.ToString())) continue;

            if (type == ThreadType.Bestiary)
                ImGui.TextWrapped("The ‘Area is invaded by red beasts’ highlight is experimental and has yet to be confirmed in-game. Beast invasion chance (%) is a candidate stat for this outcome.");

            foreach (var (name, modifier) in ThreadCatalog.Modifiers)
            {
                if (modifier.Type != type) continue;

                var mode = Settings.DisplayMode(name);
                ImGui.SetNextItemWidth(100);
                if (ImGui.BeginCombo($"{modifier.Label}##{name}", mode.ToString()))
                {
                    foreach (var option in DisplayModes)
                    {
                        if (ImGui.Selectable(option.ToString(), option == mode))
                            Settings.Mods[name] = option;
                    }
                    ImGui.EndCombo();
                }
            }
            ImGui.TreePop();
        }
        ImGui.TreePop();
    }

    public override void AreaChange(AreaInstance area)
    {
        Volatile.Write(ref _snapshot, null);
        _refresh.Reset();
    }

    public override Job Tick()
    {
        try
        {
            var area = GameController.Area.CurrentArea;
            if (!Settings.Enable || !GameController.InGame || area == null || area.IsTown || area.IsHideout
                || area.RealLevel < MinimumAreaLevel)
            {
                Volatile.Write(ref _snapshot, null);
                _refresh.Reset();
                return null;
            }

            var hash = GameController.IngameState.Data.CurrentAreaHash;
            if (_snapshot?.Hash == hash && _refresh.IsRunning && _refresh.ElapsedMilliseconds < 500)
                return null;

            _refresh.Restart();
            var data = GameController.IngameState.Data;
            var all = data.MapStats;
            var visible = data.MapStatsVisible;
            if (all == null || visible == null)
            {
                Volatile.Write(ref _snapshot, new(hash, area.DisplayName, [], "Area stats are not readable yet."));
                return null;
            }

            // Include visible-only keys as diagnostics; never assume the two dictionaries are identical subsets.
            var rows = all.Keys.Union(visible.Keys).Select(key =>
            {
                var isVisible = visible.TryGetValue(key, out var visibleValue);
                var value = all.TryGetValue(key, out var fullValue) ? fullValue : visibleValue;
                return new Row(key.ToString(), value, isVisible, isVisible ? visibleValue : null);
            }).Where(row => row.Value != 0 || row.VisibleValue.GetValueOrDefault() != 0)
                .OrderBy(row => row.Visible).ThenBy(row => row.Name, StringComparer.Ordinal).ToArray();

            // Discard a read that crossed an area transition.
            if (data.CurrentAreaHash != hash)
            {
                Volatile.Write(ref _snapshot, null);
                return null;
            }
            Volatile.Write(ref _snapshot, new(hash, area.DisplayName, rows, null));
        }
        catch (Exception)
        {
            // Memory reads can fail while loading. Never keep the previous map's results on screen.
            Volatile.Write(ref _snapshot, null);
            _refresh.Reset();
        }
        return null;
    }

    public override void Render()
    {
        var snapshot = Volatile.Read(ref _snapshot);
        if (!Settings.Enable || snapshot == null || !GameController.InGame)
            return;
        // Prevent showing a previous area's snapshot during the gap before the next Tick.
        try
        {
            if (GameController.IngameState.Data.CurrentAreaHash != snapshot.Hash) return;
        }
        catch { return; }

        var lines = new List<(string Text, Color Colour)> { ($"Map modifiers - {snapshot.Area}", HeadingColor) };
        if (snapshot.Error != null)
            lines.Add((snapshot.Error, Color.White));
        else
        {
            var threadCount = snapshot.Rows.FirstOrDefault(row => row.Name == ThreadCatalog.ThreadCountStat);
            if (threadCount != null) lines.Add((OverlayLabel(threadCount), Color.White));

            foreach (var type in Enum.GetValues<ThreadType>())
            {
                var always = Settings.Threads.AlwaysShow(type);
                var rows = snapshot.Rows.Where(row => ThreadCatalog.HasExplicitModifier(row.Name)
                    && ThreadCatalog.Matches(type, row.Name)
                    && Settings.DisplayMode(row.Name) != ModDisplayMode.Hide
                    // This stat has a baseline of one even without a Bestiary Thread.
                    && !(row.Name == "MapBestiaryBeastsXAdditionalModifiers" && row.Value == 1)
                    && (always || Settings.IncludeVisibleStats.Value || !row.Visible)).ToArray();
                if (!always && rows.Length == 0) continue;
                lines.Add((type.ToString(), HeadingColor));
                if (rows.Length == 0) lines.Add(("No matching modifiers to display", Color.White));
                foreach (var row in rows)
                {
                    var colour = Settings.DisplayMode(row.Name) == ModDisplayMode.Highlight
                        ? Settings.HighlightColour.Value : Color.White;
                    lines.Add((OverlayLabel(row), colour));
                }
            }
            if (lines.Count == 1) lines.Add(("No matching modifiers to display", Color.White));
        }
        DrawOverlay(lines);
        if (Settings.ShowInspector) RenderInspector();
    }

    private string OverlayLabel(Row row) =>
        $"{(Settings.ShowRawNames.Value ? row.Name : ThreadCatalog.Label(row.Name))}: {row.Value}";

    private void DrawOverlay(List<(string Text, Color Colour)> lines)
    {
        var width = Settings.BoxWidth.Value;
        const int padding = 10;
        var wrapped = new List<(string Text, Color Colour)>();
        foreach (var line in lines)
        {
            // Fit even long raw identifiers without drawing beyond the box.
            var remaining = line.Text;
            while (remaining.Length > 0)
            {
                var count = remaining.Length;
                while (count > 1 && Graphics.MeasureText(remaining[..count]).X > width - padding * 2) count--;
                if (count < remaining.Length)
                {
                    var space = remaining.LastIndexOf(' ', count - 1, count);
                    if (space > 0) count = space;
                }
                wrapped.Add((remaining[..count], line.Colour));
                remaining = remaining[count..].TrimStart();
            }
        }
        var max = Settings.MaximumLines.Value;
        if (wrapped.Count > max)
        {
            var omitted = wrapped.Count - max + 1;
            wrapped = wrapped.Take(max - 1).ToList();
            wrapped.Add(($"+ {omitted} more lines (open inspector)", Color.White));
        }
        var lineHeight = Graphics.MeasureText("Ag").Y + 3;
        var start = new Vector2(Settings.BoxPositionX.Value, Settings.BoxPositionY.Value);
        Graphics.DrawBox(start, start + new Vector2(width, padding * 2 + wrapped.Count * lineHeight),
            new Color(0, 0, 0, Settings.BackgroundOpacity.Value));
        for (var i = 0; i < wrapped.Count; i++)
            Graphics.DrawText(wrapped[i].Text, start + new Vector2(padding, padding + i * lineHeight),
                wrapped[i].Colour);
    }

    private void RenderInspector()
    {
        var snapshot = Volatile.Read(ref _snapshot);
        if (!Settings.Enable || snapshot == null || !GameController.InGame)
            return;

        ImGui.SetNextWindowSize(new Vector2(680, 430), ImGuiCond.FirstUseEver);
        if (ImGui.Begin("Thread modifiers###ThreadMods"))
        {
            ImGui.TextUnformatted(snapshot.Area);
            ImGui.TextWrapped("Current area stats received by the client. Hidden = absent from MapStatsVisible; source is not known.");
            var includeVisible = Settings.IncludeVisibleStats.Value;
            if (ImGui.Checkbox("Include visible", ref includeVisible)) Settings.IncludeVisibleStats.Value = includeVisible;
            ImGui.SameLine();
            var raw = Settings.ShowRawNames.Value;
            if (ImGui.Checkbox("Raw names", ref raw)) Settings.ShowRawNames.Value = raw;
            ImGui.InputText("Search", ref _search, 256);

            var rows = snapshot.Rows.Where(row => (includeVisible || !row.Visible)
                && (row.Name.Contains(_search, StringComparison.OrdinalIgnoreCase)
                    || ThreadCatalog.Label(row.Name).Contains(_search, StringComparison.OrdinalIgnoreCase))).ToArray();
            if (ImGui.Button("Copy shown stats"))
            {
                var text = new StringBuilder().AppendLine($"{snapshot.Area} | area hash {snapshot.Hash}");
                foreach (var row in rows)
                    text.AppendLine($"{row.Name} = {row.Value} | {(row.Visible ? $"visible ({row.VisibleValue})" : "hidden")}");
                ImGui.SetClipboardText(text.ToString());
            }
            ImGui.SameLine();
            ImGui.TextUnformatted($"{rows.Length} shown / {snapshot.Rows.Length} nonzero stats");
            if (snapshot.Error != null)
                ImGui.TextWrapped(snapshot.Error);
            else if (rows.Length == 0)
                ImGui.TextWrapped("No matching stats received. This does not establish that no Thread modifiers applied.");

            if (ImGui.BeginTable("Stats", 3, ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY,
                    new Vector2(0, 0)))
            {
                ImGui.TableSetupColumn("Stat", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("Raw value", ImGuiTableColumnFlags.WidthFixed, 75);
                ImGui.TableSetupColumn("Visibility", ImGuiTableColumnFlags.WidthFixed, 80);
                ImGui.TableSetupScrollFreeze(0, 1);
                ImGui.TableHeadersRow();
                foreach (var row in rows)
                {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.TextWrapped(raw ? row.Name : ThreadCatalog.Label(row.Name));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.TextUnformatted(row.Name);
                        if (row.Visible) ImGui.TextUnformatted($"MapStatsVisible value: {row.VisibleValue}");
                        ImGui.EndTooltip();
                    }
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(row.Value.ToString());
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(row.Visible ? "Visible" : "Hidden");
                }
                ImGui.EndTable();
            }
        }
        ImGui.End();
    }

}


