using System;
using System.Collections.Generic;
using System.Linq;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SpaceCrystal : WethRelicFour
{
    public override int GetAmount()
    {
        return Amount;
    }

    public override void OnCombatEnd(State state)
    {
        state.rewardsQueue.QueueImmediate(new ALoseArtifact
        {
            artifactType = Key()
        });
        base.OnCombatEnd(state);
        // state.GetCurrentQueue().QueueImmediate(new ALoseArtifact
        // {
        //     artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}"
        // });
    }
}

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class SpaceCrystalFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(SpaceCrystal);

    public override void OnReceiveArtifact(State state)
    {
        base.OnReceiveArtifact(state);
        IEnumerable<Artifact> enu = state.EnumerateAllArtifacts().Where(a => a is WethRelicFour);
        int uncounted = enu.Any(a => a is SpaceCrystal)? 0 : 1;
        foreach (Artifact relic in enu)
        {
            if (relic is WethRelicFour wrf)
            {
                wrf.UpdateStack(state, uncounted:uncounted);
            }
        }
    }
}