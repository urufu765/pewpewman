using System;
using System.Collections.Generic;
using AliasRelicStateCombat = System.Action<Weth.Artifacts.RelicCollection, State, Combat, int>;
using AliasRelicCard = System.Action<Weth.Artifacts.RelicCollection, int, Deck, Card, State, Combat, int, int, int>;
using AliasRelicEnemyHit = System.Action<Weth.Artifacts.RelicCollection, State, Combat, Part?, int>;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Weth.Artifacts;


public enum WethRelics
{
    AntiqueCell,
    DogCharm,
    PewPewGun,
    ShockStack,
    UsefulScrap,
    Omnimote,
    StubbornDrill,
    DeadFish,
    PageantRibbon,
    AstroGrass,
    StructuralStone
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class RelicCollection : Artifact
{
    public Dictionary<WethRelics, int> Relics { get; set; } = new Dictionary<WethRelics, int>
    {
        {WethRelics.AntiqueCell, 0},
        {WethRelics.DogCharm, 0},
        {WethRelics.PewPewGun, 0},
        {WethRelics.ShockStack, 0},
        {WethRelics.UsefulScrap, 0},
        {WethRelics.Omnimote, 0},
        {WethRelics.StubbornDrill, 0},
        {WethRelics.DeadFish, 0},
        {WethRelics.PageantRibbon, 0},
        {WethRelics.AstroGrass, 0},
        {WethRelics.StructuralStone, 0}
    };

    public Dictionary<WethRelics, RelicData> RelicSaveData { get; set; } = [];

    [JsonIgnore]
    public static Dictionary<WethRelics, AliasRelicStateCombat> RelicOnTurnStart { get; set; } = new Dictionary<WethRelics, AliasRelicStateCombat>
    {
        {WethRelics.AntiqueCell, AntiqueCell.DoOnTurnStartThing},
        {WethRelics.DogCharm, DogCharm.DoOnTurnStartThing},
        {WethRelics.Omnimote, Omnimote.DoOnTurnStartThing}
    };
    [JsonIgnore]
    public static Dictionary<WethRelics, AliasRelicStateCombat> RelicOnCombatStart { get; set; } = new Dictionary<WethRelics, AliasRelicStateCombat>
    {
        {WethRelics.PewPewGun, PewPewGun.DoOnCombatStartThing},
        {WethRelics.ShockStack, ShockStack.DoOnCombatStartThing}
    };
    [JsonIgnore]
    public static Dictionary<WethRelics, AliasRelicCard> RelicOnPlayerPlayCard { get; set; } = new Dictionary<WethRelics, AliasRelicCard>
    {
        {WethRelics.PewPewGun, PewPewGun.DoOnPlayerPlayCardThing}
    };
    [JsonIgnore]
    public static Dictionary<WethRelics, AliasRelicEnemyHit> RelicOnEnemyGetHit { get; set; } = new Dictionary<WethRelics, AliasRelicEnemyHit>
    {
        {WethRelics.ShockStack, ShockStack.DoOnEnemyGetHit}
    };

    public void ObtainRelic(WethRelics relic)
    {
        if (Relics.ContainsKey(relic)) Relics[relic]++;

        // Do individual stuff that won't fit here
    }

    public int GetRelicCount(WethRelics relic)
    {
        if (Relics.TryGetValue(relic, out int value)) return value;
        return 0;
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        foreach (WethRelics relic in RelicOnCombatStart.Keys.Where(relic => GetRelicCount(relic) > 0))
        {
            // Skip if there's a relic that's yet to be added, so it doesn't do duplicate actions.
            if (state.EnumerateAllArtifacts().Any(a => a.GetType() == ModEntry.NewRelicTypes[relic])) continue;

            RelicOnCombatStart[relic](this, state, combat, GetRelicCount(relic));
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        foreach (WethRelics relic in RelicOnTurnStart.Keys.Where(relic => GetRelicCount(relic) > 0))
        {
            // Skip if there's a relic that's yet to be added, so it doesn't do duplicate actions.
            if (state.EnumerateAllArtifacts().Any(a => a.GetType() == ModEntry.NewRelicTypes[relic])) continue;

            RelicOnTurnStart[relic](this, state, combat, GetRelicCount(relic));
        }
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        foreach (WethRelics relic in RelicOnEnemyGetHit.Keys.Where(relic => GetRelicCount(relic) > 0))
        {
            RelicOnEnemyGetHit[relic](this, state, combat, part, GetRelicCount(relic));
        }
    }

    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        foreach (WethRelics relic in RelicOnPlayerPlayCard.Keys.Where(relic => GetRelicCount(relic) > 0))
        {
            RelicOnPlayerPlayCard[relic](this, energyCost, deck, card, state, combat, handPosition, handCount, GetRelicCount(relic));
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tt = [];
        try
        {
            foreach (KeyValuePair<WethRelics, int> pair in Relics.Where(pair => pair.Value > 0))
            {
                tt.Add(NewWethSpaceRelics.RelicTooltip(ModEntry.NewRelicTypes[pair.Key], pair.Value, micro: true));
                tt.Add(new TTTTTTText(" "));
            }
        }
        catch (KeyNotFoundException knf)
        {
            ModEntry.Instance.Logger.LogError(knf, "This shouldn't be happening, but someone added an extra value to an unsupported relic when generating mini tooltips.");
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Something went wrong with getting mini extra tooltips!");
        }

        if (tt.Count > 0)
        {
            tt.Add(new TTDivider());
            try
            {
                tt.AddRange(Relics
                    .Where(pair => pair.Value > 0)
                    .Select(pair => NewWethSpaceRelics.RelicTooltip(ModEntry.NewRelicTypes[pair.Key], pair.Value))
                );
            }
            catch (KeyNotFoundException knf)
            {
                ModEntry.Instance.Logger.LogError(knf, "This shouldn't be happening, but someone added an extra value to an unsupported relic when generating the full tooltips.");
            }
            catch (Exception err)
            {
                ModEntry.Instance.Logger.LogError(err, "Something went wrong with getting full extra tooltips!");
            }

        }
        return tt;
    }
}