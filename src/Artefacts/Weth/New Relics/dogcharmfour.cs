using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class DogCharm : WethRelicFour
{
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            combat.QueueImmediate(new AStatus
            {
                status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive,
                statusAmount = Amount,
                targetPlayer = false,
                artifactPulse = Key()
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive, 1));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class DogCharmFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(DogCharm);
}