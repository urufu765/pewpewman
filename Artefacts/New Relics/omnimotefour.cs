using System;
using System.Collections.Generic;
using Weth.External;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class Omnimote : WethRelicFour
{
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn <= Amount)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = Key(),
                statusPulse = ModEntry.Instance.NewRelicStatuses[GetType()]
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("status.droneShift", [1]));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class OmnimoteFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(Omnimote);
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
        if (args.Status == ModEntry.Instance.NewRelicStatuses[typeof(Omnimote)]) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.NewRelicStatuses[typeof(Omnimote)]) return null;

        return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeTextStatusInfoRenderer($"{args.Amount - args.Combat.turn}").SetColor((args.Amount - args.Combat.turn > 0) ? new Color("00a94e") : ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor);
    }
}
