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
        typeof(SR2Subsuming)
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
            if (!s.EnumerateAllArtifacts().Any(a => a is TreasureHunter) || s.EnumerateAllArtifacts().Any(a => a is TreasureSeeker))
            {
                __result.Add(typeof(TreasureSeeker));
            }
            if (s.EnumerateAllArtifacts().Any(a => a is TheTerminus))
            {
                __result.Add(typeof(TerminusJaunt));
                __result.Add(typeof(TerminusMilestone));
            }
            __result = [.. __result, .. hideByDefault];
            // if (s.EnumerateAllArtifacts().Find(a => a is SR2Focused) is SR2Focused sr2)
            // {
            //     IEnumerable<Artifact> standbyRelics = s.EnumerateAllArtifacts().Where(a => a is RelicShield);
            //     if (sr2.AtMax || sr2.ObtainedRelics.Count + standbyRelics.Count() >= SR2Focused.RELICLIMIT)
            //     {
            //         IEnumerable<Type> obtainedRelics = sr2.ObtainedRelics.Select(status => sr2.RelicDic[status]);
            //         __result = [
            //             .. __result,
            //             .. allPossibleRelicTypes.Where(t => !obtainedRelics.Contains(t))
            //         ];
            //     }
            // }
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
        HashSet<Type> illegals = [.. otherRelics.Select(a => a.GetType())];
        foreach (Status status in spaceRelic.ObtainedRelics)
        {
            if (!illegals.Contains(SR2Focused.RelicDic[status]) && Activator.CreateInstance(SR2Focused.RelicDic[status]) is Artifact artifact)
            {
                a.Add(artifact);
            }
        }
        return a;
    }
}
