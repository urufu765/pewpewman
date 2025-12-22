using System;
using System.Collections.Generic;
using Shockah.Kokoro;
using Weth.External;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class AntiqueCell : WethRelicFour
{
    public const int TURNS = 3;


    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn % TURNS == 0)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.energyFragment,
                statusAmount = Amount,
                targetPlayer = true,
                artifactPulse = Key(),
                statusPulse = ModEntry.Instance.NewRelicStatuses[GetType()]
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("status.energyFragment", [1]));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class AntiqueCellFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(AntiqueCell);
}

public class AntiqueCellStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public AntiqueCellStatus()
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
        if (args.Status == ModEntry.Instance.NewRelicStatuses[typeof(AntiqueCell)]) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.NewRelicStatuses[typeof(AntiqueCell)]) return null;
        List<Color> SegmentColours = [];

        for (int i = 0; i < AntiqueCell.TURNS - 1; i++)
        {
            SegmentColours.Add(
                (i < args.Combat.turn % AntiqueCell.TURNS)
                ? new Color("00f9ff")
                : ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor
            );
        }

        return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(SegmentColours).SetRows(2).SetHorizontalSpacing(-1);
    }
}
