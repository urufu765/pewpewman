using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class StubbornDrill : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }

    public override void OnAsteroidIsDestroyed(State state, Combat combat, bool wasPlayer, int worldX)
    {
        if (wasPlayer)
        {
            combat.QueueImmediate([
                new AAddCard
                {
                    card = new TrashAutoShoot{ unplayableOverride = Special? false : null, unplayableOverrideIsPermanent = Special },
                    artifactPulse = Key()
                },
            ]);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTCard{card = new TrashAutoShoot{ unplayableOverride = Special? false : null, unplayableOverrideIsPermanent = Special }});
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class StubbornDrillFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(StubbornDrill);

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTCard{card = new TrashAutoShoot{ unplayableOverride = Special? false : null, unplayableOverrideIsPermanent = Special }});
        return tips;
    }
}