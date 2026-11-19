using System.Collections.Generic;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace ThreadMods;

public enum ModDisplayMode { Normal, Highlight, Hide }

public sealed class ThreadModsSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new(true);
    public RangeNode<int> BoxPositionX { get; set; } = new(20, 0, 7680);
    public RangeNode<int> BoxPositionY { get; set; } = new(200, 0, 4320);
    public RangeNode<int> BoxWidth { get; set; } = new(420, 200, 1200);
    public RangeNode<int> BackgroundOpacity { get; set; } = new(180, 0, 255);
    public RangeNode<int> MaximumLines { get; set; } = new(30, 5, 100);
    public ToggleNode IncludeVisibleStats { get; set; } = new(false);
    public ToggleNode ShowRawNames { get; set; } = new(false);
    public ThreadDisplaySettings Threads { get; set; } = new();
    public ToggleNode ShowInspector { get; set; } = new(false);
    [Menu("Highlight colour")]
    public ColorNode HighlightColour { get; set; } = new(Color.Green);

    [IgnoreMenu]
    public Dictionary<string, bool> HighlightedModifiers { get; set; } = new();

    [IgnoreMenu]
    public Dictionary<string, ModDisplayMode> Mods { get; set; } = new();

    internal ModDisplayMode DisplayMode(string name)
    {
        // An explicit Normal selection must override a legacy highlight too.
        if (Mods.TryGetValue(name, out var mode))
            return mode is ModDisplayMode.Highlight or ModDisplayMode.Hide ? mode : ModDisplayMode.Normal;
        return HighlightedModifiers.GetValueOrDefault(name) ? ModDisplayMode.Highlight : ModDisplayMode.Normal;
    }
}

[Submenu]
public sealed class ThreadDisplaySettings
{
    public ToggleNode AlwaysShowBestiary { get; set; } = new(false);
    public ToggleNode AlwaysShowAbyss { get; set; } = new(false);
    public ToggleNode AlwaysShowBetrayal { get; set; } = new(false);
    public ToggleNode AlwaysShowBlight { get; set; } = new(false);
    public ToggleNode AlwaysShowBreach { get; set; } = new(false);
    public ToggleNode AlwaysShowExpedition { get; set; } = new(false);
    public ToggleNode AlwaysShowIncursion { get; set; } = new(false);
    public ToggleNode AlwaysShowLegion { get; set; } = new(false);
    public ToggleNode AlwaysShowRitual { get; set; } = new(false);
    public ToggleNode AlwaysShowUltimatum { get; set; } = new(false);

    internal bool AlwaysShow(ThreadType type) => type switch
    {
        ThreadType.Bestiary => AlwaysShowBestiary.Value,
        ThreadType.Abyss => AlwaysShowAbyss.Value,
        ThreadType.Betrayal => AlwaysShowBetrayal.Value,
        ThreadType.Blight => AlwaysShowBlight.Value,
        ThreadType.Breach => AlwaysShowBreach.Value,
        ThreadType.Expedition => AlwaysShowExpedition.Value,
        ThreadType.Incursion => AlwaysShowIncursion.Value,
        ThreadType.Legion => AlwaysShowLegion.Value,
        ThreadType.Ritual => AlwaysShowRitual.Value,
        ThreadType.Ultimatum => AlwaysShowUltimatum.Value,
        _ => false
    };
}
