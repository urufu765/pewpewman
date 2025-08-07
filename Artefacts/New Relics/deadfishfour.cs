using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class DeadFish : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            combat.QueueImmediate([
                new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true
                },
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive,
                    statusAmount = Special? 2 : 5,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            ]);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("status.powerdrive", [1]));
        tips.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive, Special? 2 : 5));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class DeadFishFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(DeadFish);
}