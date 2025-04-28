using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace Weth.Artifacts;

public static class Artifacthider
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(ArtifactReward).GetMethod("GetBlockedArtifacts", AccessTools.all),
            postfix: new HarmonyMethod(typeof(Artifacthider), nameof(ArtifactRewardPreventer))
        );
    }

    /// <summary>
    /// GetBlockedArtifacts postfix
    /// </summary>
    /// <param name="s"></param>
    /// <param name="__result"></param>
    private static void ArtifactRewardPreventer(State s, ref HashSet<Type> __result)
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
            __result.Add(typeof(SpaceRelics));
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Fuck, fucked up adding stuff to the list of don'ts");
        }
    }
}
