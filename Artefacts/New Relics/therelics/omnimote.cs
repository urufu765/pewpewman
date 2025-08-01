using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;
using Weth.External;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.Omnimote)]
public class Omnimote : NewWethSpaceRelics
{
    public static void DoOnTurnStartThing(RelicCollection r, State s, Combat c, int n)
    {
        s.ship.Set(ModEntry.Instance.Relic_Omnimote.Status, n);
        
        if (c.turn <= n)
        {
            c.QueueImmediate(new AStatus
            {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("status.droneShift", ["1"]));
        return tips;
    }
}

public class OmnimoteStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public OmnimoteStatus()
    {
        ModEntry.Instance.KokoroApi.V2.StatusLogic.RegisterHook(this);
        ModEntry.Instance.KokoroApi.V2.StatusRendering.RegisterHook(this);
    }

    /// <summary>
    /// No relic statuses should be affected by timestop nor boost
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public bool? IsAffectedByBoost(IKokoroApi.IV2.IStatusLogicApi.IHook.IIsAffectedByBoostArgs args)
    {
        if (args.Status == ModEntry.Instance.Relic_Omnimote.Status) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.Relic_Omnimote.Status) return null;
        List<Color> SegmentColours = [];

        for (int i = 0; i < args.Amount; i++)
        {
            SegmentColours.Add(
                (i < args.Amount - args.Combat.turn)
                ? new Color("00a94e")
                : ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor
            );
        }

        return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(SegmentColours);
    }
}