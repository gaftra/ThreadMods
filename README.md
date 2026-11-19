# ThreadMods

Read-only ExileAPI overlay for the current area's client-received stats, including stats omitted from `IngameState.Data.MapStatsVisible`.

## Use

Reload/restart ExileAPI so it builds this source plugin, then enable **ThreadMods** in the plugin menu. The click-through information box appears in areas of level 68 or higher outside towns and hideouts, and refreshes twice per second.

## Progress

- [ ] Abyss
- [ ] Bestiary 
- [ ] Betrayal
- [ ] Blight
- [ ] Breach
- [ ] Expedition
- [X] Incursion
- [ ] Legion
- [ ] Ritual
- [ ] Ultimatum

## Guaranteed Thread bonuses

The catalogue includes the guaranteed bonuses below. Values in the 3/6/8 columns are the expected bonuses supplied for this update, not values inferred by the plugin. The overlay displays the actual nonzero stats received from the client; other sources can contribute to those totals. Bestiary and Legion reuse existing catalogue entries. Ultimatum uses two separate stats.

| Mechanic / bonus | 3 Threads | 6 Threads | 8 Threads | ExileAPI stat |
| --- | --- | --- | --- | --- |
| Abyss: additional Pits | 1 | 2 | 3 | `MapAbyssNumAdditionalPits` |
| Breach: increased Magic monsters | 20% | 40% | 60% | `MapBreachNumberOfMagicPacksPct` |
| Blight: Surpassing Chance for a lane to be at least Magic | +40% | +80% | +120% | `MapBlightLanesSpawnAtLeastMagicMonstersSurpassChancePct` |
| Legion: additional encounters | 1 | 2 | 3 | `MapLegionLeagueExtraSpawns` |
| Expedition: Surpassing Chance for an additional Remnant Suffix | 15% | 30% | 50% | `MapExpeditionExtraRelicSuffixChancePct` |
| Ultimatum: increased Rare monsters | 15% | 15% | 30% | `MapUltimatumRareMonsterQuantityPct` |
| Ultimatum: rewards as though additional rounds completed | 0 | 1 | 1 | `MapUltimatumRewardTiers` |
| Ritual: increased Favours offered | 20% | 40% | 60% | `MapRitualRewardCountPct` |
| Bestiary: additional Red Beasts | 2 | 3 | 4 | `MapAdditionalRedBeasts` |
| Betrayal: increased chance for reinforcements | 30% | 60% | 90% | `MapBetrayalSecondaryTargetAmountPct` |
| Incursion: increased Pack Size | 20% | 30% | 40% | `MapIncursionPackSizePct` |

All identifiers above exist in the installed ExileAPI `GameStat` enum. Available local stat descriptions support the Abyss, Legion, Bestiary, Betrayal, Expedition, Ritual and Ultimatum mappings. Breach, Blight and Incursion are mapped by their enum names; their correspondence to the new bonuses still needs live verification. The local Expedition description calls this a chance rather than Surpassing Chance; the label follows the supplied update wording. None of these additions establishes live receipt or attribution to Threads.

`MapNumExtraAbysses` counts additional Abyss encounters and remains a separate entry from additional Pits. Guaranteed modifiers also appear in the **Mods** settings. Enable **Include Visible Stats** or the category's **Always Show** toggle if a bonus is in the visible stats dictionary.

## Thread modifier checklist

Check an item when its Thread outcome has been confirmed. Listed entries are stats the plugin can surface as candidates; a matching stat is not proof that the Thread caused it. **N/A** means no specific outcomes are listed yet, though the plugin can still surface stats whose names match that category's keywords.

### Abyss

- [ ] An Abyssal Pit spawns an Abyssal Consort. `MapFinalAbyssPitSpawnAbyssMiniboss` is the likely mapping and uses its internal wording in the overlay.
- [ ] The area gains additional Abyssal Pits
- [ ] Rare monsters from Abyssal Pits spawn in pairs
- [ ] Abyssal Pits fire mortars that spawn Bone Hoarders on impact
- [ ] Rare monsters from Abyssal Pits gain an additional Abyssal modifier

### Bestiary

- [X] Additional red beasts
- [ ] Beast invasion chance (Thread outcome mapping unconfirmed)
- [ ] Area contains the Black Morrigan
- [X] Einhar blood explosion aura
- [X] Beasts empowered by wisps of all types
- [ ] Yellow beasts replaced by hulks (internal name; tooltip mapping unconfirmed)
- [ ] Bestiary packs replaced by Harvest beasts (internal name; tooltip mapping unconfirmed)

### Betrayal

- [ ] Catarina accompanies a Syndicate encounter
- [X] Dealing with downed Syndicate members grants powerful buffs to players and mercenaries
- [ ] Catarina's Corpse Sculpture appears in the area
- [X] Additional rare monster modifiers on Syndicate members
- [ ] Chance for dropped Veiled items to have their Veiled modifier fractured (%)
- [X] Additional clusters of Syndicate Safehouse barrels and chests

### Blight

- [X] Blight pump and towers spawn friendly Pustules that grant auras
- [ ] Defending the Blight pump spawns a miniboss whose rewards improve with Blight kills
- [X] Blight lanes replaced by rolling currency monsters
- [X] Blight lanes replaced by Rogue Exiles that each drop a Unique item
- [ ] Defending the replacement Pantheon God awakens it; Blight kills improve its drops, and at least one rare is Pantheon-Touched
- [X] Up to two lanes are Mycotic
- [ ] Building and upgrading Blight towers is free (general tower-cost stat only; Thread mapping unconfirmed)

### Breach

- [X] Breach Hives lead to a Hive Fortress 
- [ ] Unstable Breaches contain a boss 
- [X] The area contains an additional Unstable Breach. This can roll up to three times 
- [ ] Rare Breach monsters drop an additional Wombgift when killed (displayed as a raw percentage chance)
- [X] The area contains an additional Genesis Tree Sapling 
- [X] Breach Hive and Fortress encounters contain additional Breach Coffers
- [X] Rare Breach monsters have an additional Breach modifier
- [ ] Ailith's skills in the area are empowered

### Expedition

- [X] The Expedition encounter is massively larger (displayed as the raw percentage stat)
- [ ] All Runic Monsters in the encounter are at least Rare
- [ ] A Runic Obelisk spawns and can revive all Rare and Magic Expedition monsters when activated
- [ ] The encounter contains at least one chest that can be unearthed and drops a Logbook
- [ ] The encounter contains markers for Runic Lieutenants, which drop extra reroll currencies when slain
- [ ] Expedition Remnants have additional upside affixes

### Incursion

- [X] Doors can be golden. Opening one upgrades the tier of the connected room
- [X] Rooms contain an object that opens all locked doors
- [X] Rooms contain Vaal Vessels
- [X] Rooms contain Gold Piles
- [X] Claiming Cursed Treasures drops stacks of currency and reduces the remaining Incursion time
- [X] Rooms contain a Vaal Flesh Merchant

### Legion

- [X] The area contains additional Legion encounters
- [X] Both Legions have Generals
- [ ] War Hoards replace all Legion chests
- [X] Purple crystals cover the battlefield. Defeating high-ranking Legion monsters shatters nearby crystals and drops items
- [ ] Lower-ranking Legion monsters are promoted
- [X] High-ranking Legion monsters are positioned close to the Monolith
- [ ] Legion monsters can be released from stasis repeatedly, gaining difficulty and rewards with each release

### Ritual

- [X] Rituals in the area are Nameless
- [X] Ritual Altars spawn a friendly wisp. Standing near it increases Tribute gained from slain enemies
- [X] Completing the final Ritual turns it into a Bodach's Ritual that can be run again for more Tribute
- [X] Completing a Ritual grants every player a Blood Shrine buff 
- [X] At the final Ritual, players can sacrifice their Tribute to summon a champion of the King

### Ultimatum

- [X] All offered rewards are Inscribed Ultimatums
- [ ] All offered rewards are Catalysts
- [ ] The encounter leads to the Trialmaster
- [X] A beating heart spawns every round. Defeating it temporarily grants Adrenaline to all players and mercenaries in the area
- [ ] Double Down is available at round 7 or 8
- [X] Cursed Currency spawns every round. Claiming it adds random Currency to the collected rewards
- [ ] The encounter spawns a nearby Soul Core. Claiming it speeds up completion for all Ultimatum encounter types and can empower Ultimatum enemies
- [ ] Each round offers secondary upsides alongside the usual difficulty modifiers

Set **Box Position X/Y**, **Box Width**, **Background Opacity**, and **Maximum Lines** in the plugin settings. Text wraps to the box width and uses ExileAPI's configured font. Overflow is indicated; use the inspector to see everything.

The overlay shows the `MapTotalThreadCount` stat as **Thread count**, plus only exact modifier entries listed in `ThreadCatalog.cs`. Keyword matches and other unclassified stats remain available in the inspector. The **Threads** settings contain **Always Show** toggles for Bestiary, Abyss, Betrayal, Blight, Breach, Expedition, Incursion, Legion, Ritual, and Ultimatum. All are off by default. An enabled toggle keeps that category heading present and includes its visible as well as hidden nonzero mapped stats, except modifiers set to **Hide**. With no displayable modifiers it says "No matching modifiers to display"; it never invents an active effect. Categories appear automatically when matching hidden mapped stats arrive and have not been set to **Hide**. These settings apply inside areas, not towns or hideouts.

**Include Visible Stats** includes visible entries for all mapped categories.

Expand **Mods** and a category to choose **Highlight**, **Normal**, or **Hide** from each modifier's dropdown. **Highlight** uses the configured **Highlight colour** (green by default), **Normal** uses white, and **Hide** omits the modifier from the overlay even with **Always Show** enabled. The inspector and copied stats still include hidden modifiers for diagnostics. New modifiers default to **Normal**, and existing highlight choices are preserved. The Bestiary section marks the red-beast invasion highlight as experimental and unconfirmed in-game.

**Show Inspector** opens the searchable diagnostic window in eligible areas. Its visibility filter is independent of the category Always Show overrides.

- **Include visible** also shows entries in the visible dictionary.
- **Raw names** and hovering a label expose the exact API identifier.
- **Search** matches labels and raw identifiers.
- **Copy shown stats** copies the filtered results, values and area hash for comparison.

## What has been verified

The installed ExileCore.dll exposes both dictionaries as `Dictionary<GameStat, int>`. Its GameStat enum includes `MapBestiaryAreaContainsBlackMorrigan`, `MapBestiaryEinharBloodExplodeAura`, `MapBestiaryBeastsHaveXWispsOfAllTypes`, `MapAdditionalRedBeasts`, `MapBestiaryYellowBeastsAreHulks`, and `MapXBestiaryPacksAreHarvestBeasts`.

These names establish API support, not that every rolled Thread effect is sent by the server. Live receipt still needs validation in a Thread-modified map. The last two identifiers retain internal wording because their correspondence to Nameless Beasts and Sacred Fauna has not been verified.

The DLL also includes `MapEinharMissionBeastInvasionChancePct` and `DisplayMemoryLineBestiaryGreatMigration`. The former is shown as **Beast invasion chance (%)**, separately from **Additional red beasts**. Neither has been confirmed as the specific Thread outcome "Area is invaded [by] a large number of Red Beasts". A chance stat is not a confirmed outcome. Missing entries can mean zero, not received, or filtered; the old plugin already discovered these identifiers generically when present.

## Blight Thread modifiers

The in-game modifier tooltip lists these outcomes. All seven identifiers below are in the installed ExileAPI `GameStat` enum. The overlay labels the last two as candidates and displays their raw values when present.

| Tooltip effect | ExileAPI stat |
| --- | --- |
| The Blight Pump and towers spawn friendly Pustules that grant auras. | `MapBlightSpawnFriendlyPustules` |
| Successfully defending the pump spawns a miniboss. Defeating more Blight enemies improves the quantity and quality of its rewards. | `MapBlightCoreSpawnMiniboss` |
| One lane spawns rolling currency monsters in place of its usual monsters. | `MapBlightReplaceXLanesWithCurrencyLane` |
| One lane spawns Rogue Exiles in place of its usual monsters. Each Exile always drops a Unique item. | `MapBlightReplaceXLanesWithExileLane` |
| A slumbering Pantheon God replaces the pump and awakens after a successful defense. More Blight kills improve the quantity and quality of its drops. At least one rare monster in the encounter is Pantheon-Touched. | `MapBlightCoreReplacePantheonGod` |
| Up to two lanes are Mycotic. | `MapBlightUpToXSuperchargedLanes` is a candidate. The installed data does not tie that identifier to the Mycotic tooltip. |
| Building and upgrading Blight towers is free. | The installed data has a general `MapBlightTowerCostPct` stat. The specific Thread mapping and raw value are unconfirmed. |

In **Threads**, enable the Blight **Always Show** toggle to include visible Blight stats in the overlay. To capture candidate values, open the inspector, enable **Include visible**, and search for `Blight` after entering a Blight Thread map. **Copy shown stats** captures the raw identifiers and values for comparison with the lane types and tower costs you observe.

## Extending the modifier catalogue

`ThreadCatalog.cs` owns mechanic categories, fallback keyword matching, and exact stat-to-label mappings. Add confirmed modifiers to its `Modifiers` dictionary with the correct `ThreadType` and label; no overlay changes are required. Exact mappings take precedence over keyword matching. Stats without an exact mapping use readable internal stat names in the inspector. Keyword grouping can include related mechanic stats that are not Thread rolls, and a cross-mechanic stat can appear in multiple groups. Unmapped names remain available in the inspector but are not shown in the overlay.

This reads the entered area, not an unopened map item or a prediction of the random roll. Hidden means the key is absent from MapStatsVisible; it is not proof of a particular in-game tooltip's behavior. Stats can originate from other sources, and aggregate values do not identify which source applied them. Values are raw integers, not universally percentages or counts. Zero entries are omitted. Empty results do not prove absence of an effect. Server-only effects cannot be recovered through these dictionaries.

## Build

From this directory: `dotnet build -c Release`. References resolve to the ExileAPI folder three levels above this plugin, or to `exapiPackage` when provided by the loader. No additional NuGet packages are required.

## Live verification

Enter a map activated with Bestiary Threads, enable **Include visible** in the inspector, and copy the results. Compare the raw identifiers with observed encounters/effects. Repeat in a fresh map without those Threads while keeping other map sources as similar as possible. This helps identify candidates but does not by itself establish the cause of aggregate stats.
