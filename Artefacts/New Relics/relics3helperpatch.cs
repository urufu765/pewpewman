using System.Collections.Generic;

namespace Weth.Artifacts;

public static class FixTheRelicTooltips
{
    public static void FixIt(ref List<Tooltip> __result, Artifact __instance)
    {
        if (__instance is NewWethSpaceRelics nws)
        {
            if (MG.inst?.g?.state?.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc)
            {
                __result[0] = NewWethSpaceRelics.RelicTooltip(__instance.GetType(), rc.GetRelicCount(nws.GetThing()) + 1, false);
            }
            else
            {
                __result[0] = NewWethSpaceRelics.RelicTooltip(__instance.GetType(), 1, false);
            }
        }
    }
}