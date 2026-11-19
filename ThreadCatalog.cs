using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThreadMods;

internal enum ThreadType { Bestiary, Abyss, Betrayal, Blight, Breach, Expedition, Incursion, Legion, Ritual, Ultimatum }

internal static class ThreadCatalog
{
    internal const string ThreadCountStat = "MapTotalThreadCount";

    internal sealed record Modifier(ThreadType Type, string Label);

    // Use exact identifiers and mark unconfirmed Thread outcomes as candidates.
    // Grouping describes the mechanic, not proof that a Thread caused the stat.
    internal static readonly IReadOnlyDictionary<string, Modifier> Modifiers = new Dictionary<string, Modifier>
    {
        ["MapAdditionalRedBeasts"] = new(ThreadType.Bestiary, "Additional red beasts"),
        ["MapBestiaryBeastsXAdditionalModifiers"] = new(ThreadType.Bestiary, "Additional Bestiary modifiers on Red Beasts (candidate)"),
        ["MapEinharMissionBeastInvasionChancePct"] = new(ThreadType.Bestiary, "Beast invasion chance (%)"),
        ["DisplayMemoryLineBestiaryGreatMigration"] = new(ThreadType.Bestiary, "Bestiary Great Migration (memory display stat)"),
        ["MapBestiaryAreaContainsBlackMorrigan"] = new(ThreadType.Bestiary, "Area contains the Black Morrigan"),
        ["MapBestiaryEinharBloodExplodeAura"] = new(ThreadType.Bestiary, "Einhar blood explosion aura"),
        ["MapBestiaryBeastsHaveXWispsOfAllTypes"] = new(ThreadType.Bestiary, "Beasts empowered by wisps of all types"),
        ["MapBestiaryYellowBeastsAreHulks"] = new(ThreadType.Bestiary, "Yellow beasts replaced by hulks (internal name)"),
        ["MapXBestiaryPacksAreHarvestBeasts"] = new(ThreadType.Bestiary, "Bestiary packs replaced by Harvest beasts (internal name)"),
        ["MapFinalAbyssPitSpawnAbyssMiniboss"] = new(ThreadType.Abyss, "Final Abyssal Pit spawns a miniboss"),
        ["MapNumExtraAbysses"] = new(ThreadType.Abyss, "Additional Abyss encounters"),
        ["MapAbyssNumAdditionalPits"] = new(ThreadType.Abyss, "Additional Abyssal Pits"),
        ["MapAbyssEliteMonstersPaired"] = new(ThreadType.Abyss, "Rare monsters from Abyssal Pits spawn in pairs"),
        ["MapAbyssPitsFireMortars"] = new(ThreadType.Abyss, "Abyssal Pit mortars spawn Bone Hoarders"),
        ["MapAbyssRaresXAdditionalAbyssMods"] = new(ThreadType.Abyss, "Additional Abyssal modifiers on rare monsters from pits"),
        ["MapBetrayalSecondaryTargetAmountPct"] = new(ThreadType.Betrayal, "Increased chance for Syndicate reinforcements (%)"),
        ["MapBetrayalStandaloneCatarina"] = new(ThreadType.Betrayal, "Catarina accompanies a Syndicate encounter"),
        ["MapBetrayalEncounterGrantBuffs"] = new(ThreadType.Betrayal, "Dealing with downed Syndicate members grants powerful buffs to players and mercenaries"),
        ["MapBetrayalSpawnCorpseSculpture"] = new(ThreadType.Betrayal, "Catarina's Corpse Sculpture appears in the area"),
        ["MapBetrayalMembersXAdditionalMods"] = new(ThreadType.Betrayal, "Additional rare monster modifiers on Syndicate members"),
        ["MapBetrayalPctChanceVeiledItemsToDropWithFracturedVeiledMod"] = new(ThreadType.Betrayal, "Chance for dropped Veiled items to have their Veiled modifier fractured (%)"),
        ["MapBetrayalSpawnResearchChestClusters"] = new(ThreadType.Betrayal, "Additional clusters of Syndicate Safehouse barrels and chests"),
        ["MapBlightLanesSpawnAtLeastMagicMonstersSurpassChancePct"] = new(ThreadType.Blight, "Surpassing chance for Blight lanes to be at least Magic (%)"),
        ["MapBlightSpawnFriendlyPustules"] = new(ThreadType.Blight, "Blight pump and towers spawn friendly Pustules that grant auras"),
        ["MapBlightCoreSpawnMiniboss"] = new(ThreadType.Blight, "Defending the Blight pump spawns a miniboss whose rewards improve with Blight kills"),
        ["MapBlightReplaceXLanesWithCurrencyLane"] = new(ThreadType.Blight, "Blight lanes replaced by rolling currency monsters"),
        ["MapBlightReplaceXLanesWithExileLane"] = new(ThreadType.Blight, "Blight lanes replaced by Rogue Exiles that each drop a Unique item"),
        ["MapBlightCoreReplacePantheonGod"] = new(ThreadType.Blight, "Defending the replacement Pantheon God awakens it. Blight kills improve its drops, and at least one rare is Pantheon-Touched"),
        ["MapBlightUpToXSuperchargedLanes"] = new(ThreadType.Blight, "Mycotic lanes (candidate)"),
        ["MapBlightTowerCostPct"] = new(ThreadType.Blight, "Tower construction and upgrade cost (%) (candidate for free towers)"),
        ["MapBreachNumberOfMagicPacksPct"] = new(ThreadType.Breach, "Increased number of Magic Breach monsters (%)"),
        ["MapBreachHiveIsAlwaysSkyFortress"] = new(ThreadType.Breach, "Breach Hives lead to a Hive Fortress"),
        ["MapUnstableBreachSpawnsBoss"] = new(ThreadType.Breach, "Unstable Breaches contain a boss"),
        ["MapContainsAdditionalUnstableBreaches"] = new(ThreadType.Breach, "Additional Unstable Breaches"),
        ["MapBreachRareAdditionalWombgiftChancePct"] = new(ThreadType.Breach, "Chance for rare Breach monsters to drop an additional Wombgift (%)"),
        ["MapBreachContainsGenesisTreeSapling"] = new(ThreadType.Breach, "Area contains an additional Genesis Tree Sapling"),
        ["MapBreachHivesContainSpreadCoffers"] = new(ThreadType.Breach, "Additional Breach Coffers in Hive and Fortress encounters"),
        ["MapBreachRaresXAdditionalBreachMods"] = new(ThreadType.Breach, "Additional Breach modifiers on rare Breach monsters"),
        ["MapBreachEmpoweredAilithSkills"] = new(ThreadType.Breach, "Ailith's skills are empowered"),
        ["MapExpeditionExtraRelicSuffixChancePct"] = new(ThreadType.Expedition, "Surpassing chance for Remnants to have an additional Suffix (%)"),
        ["MapExpeditionEncounterScalePct"] = new(ThreadType.Expedition, "Expedition encounter scale (%)"),
        ["MapExpeditionRunicMonstersAtLeastRare"] = new(ThreadType.Expedition, "All Runic Monsters are at least Rare"),
        ["MapExpeditionEncounterSpawnReviveObelisk"] = new(ThreadType.Expedition, "Runic Obelisk can revive Rare and Magic Expedition monsters"),
        ["MapExpeditionContainsXAdditionalLogbookChests"] = new(ThreadType.Expedition, "Additional Expedition chests that drop a Logbook"),
        ["MapExpeditionLieutenantMarkerCount"] = new(ThreadType.Expedition, "Runic Lieutenant markers (extra reroll currencies)"),
        ["MapExpeditionXAdditionalSpecialRemnantModifiers"] = new(ThreadType.Expedition, "Additional upside affixes on Expedition Remnants"),
        ["MapIncursionPackSizePct"] = new(ThreadType.Incursion, "Increased Incursion pack size (%)"),
        ["MapIncursionContainsTempests"] = new(ThreadType.Incursion, "Rooms contain a random type of Tempest"),
        ["MapIncursionCanHaveGoldenDoors"] = new(ThreadType.Incursion, "Opening golden doors upgrades the connected room"),
        ["MapIncursionContainsDoorUnlockingObject"] = new(ThreadType.Incursion, "Rooms contain an object that opens all locked doors"),
        ["MapIncursionContainsXVaalVessels"] = new(ThreadType.Incursion, "Vaal Vessels in Incursion rooms"),
        ["MapIncursionContainsXGoldPiles"] = new(ThreadType.Incursion, "Gold Piles in Incursion rooms"),
        ["MapAlvaCursedTreasure"] = new(ThreadType.Incursion, "Cursed Treasures grant currency and reduce remaining time"),
        ["MapIncursionsSpawnSpecialMonster"] = new(ThreadType.Incursion, "Rooms contain a Vaal Flesh Merchant"),
        ["MapLegionLeagueExtraSpawns"] = new(ThreadType.Legion, "Additional Legion encounters"),
        ["MapLegionLeagueForceGeneral"] = new(ThreadType.Legion, "Each Legion is accompanied by a General"),
        ["MapLegionGeneralsSpawnBothGeneralsChancePct"] = new(ThreadType.Legion, "Chance for both Legions to have Generals (%)"),
        ["MapLegionAllChestsWarHoardsChancePct"] = new(ThreadType.Legion, "Chance for all Legion chests to become War Hoards (%)"),
        ["MapLegionSmallCrystalsChancePct"] = new(ThreadType.Legion, "Chance for purple crystals on the Legion battlefield (%)"),
        ["MapLegionUpgradeMonsterRankChancePct"] = new(ThreadType.Legion, "Chance to upgrade lower-rank Legion monsters (%)"),
        ["MapLegionInvertedLegionChancePct"] = new(ThreadType.Legion, "Chance for high-rank Legion monsters near the Monolith (%)"),
        ["MapLegionMultipleBreakoutChancePct"] = new(ThreadType.Legion, "Chance for repeated Legion stasis breakouts (%)"),
        ["MapRitualRewardCountPct"] = new(ThreadType.Ritual, "Increased Favours offered by Ritual Altars (%)"),
        ["MapRitualTypeIsNameless"] = new(ThreadType.Ritual, "Rituals in the area are Nameless"),
        ["RitualAltarsSpawnWisp"] = new(ThreadType.Ritual, "Ritual Altars spawn a friendly wisp that boosts nearby Tribute gains"),
        ["MapFinalRitualBodachArena"] = new(ThreadType.Ritual, "Final Ritual becomes a repeatable Bodach's Ritual"),
        ["MapRitualGainBloodShrineOnCompletion"] = new(ThreadType.Ritual, "Completing a Ritual grants all players a Blood Shrine buff"),
        ["MapRitualFinalRitualExpendTributeForBoss"] = new(ThreadType.Ritual, "Sacrifice Tribute at the final Ritual to summon a champion of the King"),
        ["MapUltimatumRareMonsterQuantityPct"] = new(ThreadType.Ultimatum, "Increased Rare monsters spawned during Ultimatum (%)"),
        ["MapUltimatumRewardTiers"] = new(ThreadType.Ultimatum, "Additional completed rounds for Ultimatum rewards"),
        ["MapUltimatumRewardsAreInscribed"] = new(ThreadType.Ultimatum, "All Ultimatum rewards are Inscribed Ultimatums"),
        ["MapUltimatumRewardsAreCatalysts"] = new(ThreadType.Ultimatum, "All Ultimatum rewards are Catalysts"),
        ["MapUltimatumLeadsToBoss"] = new(ThreadType.Ultimatum, "Ultimatum leads to the Trialmaster"),
        ["MapUltimatumSpawnAdrenalineHearts"] = new(ThreadType.Ultimatum, "Adrenaline-granting heart spawns each round"),
        ["MapUltimatumCanDoubleDownFromRoundX"] = new(ThreadType.Ultimatum, "Double Down available from round"),
        ["MapUltimatumSpawnXCursedCurrencyPerRound"] = new(ThreadType.Ultimatum, "Claimable Cursed Currency each round"),
        ["MapUltimatumSpawnSoulCoreObject"] = new(ThreadType.Ultimatum, "Claimable Soul Core speeds up the encounter and can empower enemies")
    };

    internal static bool HasExplicitModifier(string name) => Modifiers.ContainsKey(name);

    private static readonly IReadOnlyDictionary<ThreadType, string[]> Keywords = new Dictionary<ThreadType, string[]>
    {
        [ThreadType.Bestiary] = ["Bestiar", "Beast", "Einhar"],
        [ThreadType.Abyss] = ["Abyss"],
        [ThreadType.Betrayal] = ["Betrayal", "Syndicate", "JunMission"],
        [ThreadType.Blight] = ["Blight"],
        [ThreadType.Breach] = ["Breach"],
        [ThreadType.Expedition] = ["Expedition"],
        [ThreadType.Incursion] = ["Incursion", "AlvaMission"],
        [ThreadType.Legion] = ["Legion"],
        [ThreadType.Ritual] = ["Ritual"],
        [ThreadType.Ultimatum] = ["Ultimatum"]
    };

    internal static bool Matches(ThreadType type, string name) => Modifiers.TryGetValue(name, out var modifier)
        ? modifier.Type == type
        : Keywords[type].Any(keyword => name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

    internal static bool IsCategorized(string name) => Enum.GetValues<ThreadType>().Any(type => Matches(type, name));

    internal static string Label(string name) => name == ThreadCountStat ? "Thread count"
        : Modifiers.TryGetValue(name, out var modifier) ? modifier.Label
        : Regex.Replace(name.StartsWith("Map", StringComparison.Ordinal) ? name[3..] : name,
            "(?<=[a-z0-9])(?=[A-Z])", " ");
}
