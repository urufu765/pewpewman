using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace Weth.Artifacts;

public static class Artifacthider
{
    private static readonly List<Type> hideByDefault = [
        typeof(SpaceRelics),
        typeof(SpaceRelics2),
        typeof(SR2Crackling),
        typeof(SR2Focused),
        typeof(SR2Subsuming),
        typeof(SuperDriveCollector),
    ];
    // private static readonly List<Type> allPossibleRelicTypes = [
    //     typeof(RelicAutododgeRight),
    //     typeof(RelicBoost),
    //     typeof(RelicDrawNextTurn),
    //     typeof(RelicDroneShift),
    //     typeof(RelicEnergyFragment),
    //     typeof(RelicEvade),
    //     typeof(RelicFlux),
    //     typeof(RelicHermes),
    //     typeof(RelicShield),
    //     typeof(RelicPulsedrive),
    //     typeof(RelicShard),
    //     typeof(RelicStunCharge),
    //     typeof(RelicTempPayback),
    //     typeof(RelicTempShield)
    // ];

    /// <summary>
    /// GetBlockedArtifacts postfix
    /// </summary>
    /// <param name="s"></param>
    /// <param name="__result"></param>
    public static void ArtifactRewardPreventer(State s, ref HashSet<Type> __result)
    {
        try
        {
            #region WETH
            if (!s.EnumerateAllArtifacts().Any(a => a is TreasureHunter) || s.EnumerateAllArtifacts().Any(a => a is TreasureSeeker))
            {
                __result.Add(typeof(TreasureSeeker));
            }
            if (s.EnumerateAllArtifacts().Any(a => a is TheTerminus))
            {
                __result.Add(typeof(TerminusJaunt));
                __result.Add(typeof(TerminusMilestone));
            }

            // The below hides:
            /*
            __result: Everything from __result
            hideByDefault: Relic collectors (for Relics v2 and v3)
            NewRelicCounterparts.Keys: All the "real" relics (the ones that store the relic stacks)
            NewSpecialRelicCounterparts.Values: All the special fake relics (since they shouldn't be able to be drawn normally)
            */
            __result = [.. __result, .. hideByDefault, .. ModEntry.NewRelicCounterparts.Keys, .. ModEntry.NewSpecialRelicCounterparts.Values];

            // Hide Space Relics version 2 relics if version 2 is not present. (basically has to be cheated in)
            if (!s.EnumerateAllArtifacts().Any(a => a is SpaceRelics2))
            {
                __result = [.. __result, .. ModEntry.WethSpecialArtifacts];
            }

            // Prevent owned relic from showing up twice (only applies to version 2 Space Relics)
            if (s.EnumerateAllArtifacts().Find(a => a is SR2Focused) is SR2Focused sr2)
            {
                foreach (Status status in sr2.ObtainedRelics)
                {
                    if (status == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive)
                    {
                        __result.Add(typeof(RelicPulsedrive));
                    }
                    else
                    {
                        try
                        {
                            __result.Add(SR2Focused.RelicDic[status]);
                        }
                        catch (Exception err)
                        {
                            ModEntry.Instance.Logger.LogError(err, "Failed to list SR2Focused owned relic as an offering exception");
                        }
                    }
                }
            }
            #endregion

            #region ROADKILL
            if (s.EnumerateAllArtifacts().Any(a => a is Pyrotactics))
            {
                __result.Add(typeof(Pyroforger));
            }
            if (s.EnumerateAllArtifacts().Any(a => a is Pyroforger))
            {
                __result.Add(typeof(Pyrotactics));
            }
            #endregion
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Fuck, fucked up adding stuff to the list of don'ts");
        }
    }

    public static void FocusedSpaceRelicsAlwaysRelicRelic(State s, int count, Deck? limitDeck, List<ArtifactPool>? limitPools, ref List<Artifact> __result)
    {
        if (!(limitDeck == ModEntry.Instance.WethDeck.Deck && limitPools is not null && limitPools.Contains(ArtifactPool.Unreleased))) return;

        // Always offer relics you already have
        if (s.EnumerateAllArtifacts().Find(a => a is SR2Focused) is SR2Focused sr2)
        {
            // Relics that are physically present next to Weth (since duplicates don't work)
            IEnumerable<Artifact> standbyRelics = s.EnumerateAllArtifacts().Where(a => a is RelicShield);
            List<Artifact> relics = ConjureUpRelicsFromFocused(sr2, [.. standbyRelics]);

            // If player already has max allowed types of relics
            if (sr2.AtMax || sr2.ObtainedRelics.Count + standbyRelics.Count() >= SR2Focused.RELICLIMIT)
            {
                __result = [.. relics.Take(count)];
            }
            else  // put the owned Relics in front of the results
            {
                List<Artifact> result = [.. relics, .. __result];
                __result = [.. result.Take(count)];
            }
        }
    }

    private static List<Artifact> ConjureUpRelicsFromFocused(SR2Focused spaceRelic, params Artifact[] otherRelics)
    {
        List<Artifact> a = [];
        try
        {
            HashSet<Type> illegals = [.. otherRelics.Select(a => a.GetType())];
            foreach (Status status in spaceRelic.ObtainedRelics)
            {
                // Remember, always handle the Pulsedrive separately.
                if (status == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive)
                {
                    if (!illegals.Contains(typeof(RelicPulsedrive)))
                    {
                        a.Add(new RelicPulsedrive());
                    }
                }
                else if (!illegals.Contains(SR2Focused.RelicDic[status]) && Activator.CreateInstance(SR2Focused.RelicDic[status]) is Artifact artifact)
                {
                    a.Add(artifact);
                }
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Something went wrong with conjuring relics!");
        }
        return a;
    }
}
