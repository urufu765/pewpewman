using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class UsefulScrap : WethRelicFour
{
    public override void GainStack(State state, bool? special = null)
    {
        base.GainStack(state, special);
        state.GetCurrentQueue().QueueImmediate(new AHeal
        {
            healAmount = Amount,
            targetPlayer = true,
            artifactPulse = Key()
        });
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class UsefulScrapFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(UsefulScrap);

    // public override void OnReceiveArtifact(State state)
    // {
    //     base.OnReceiveArtifact(state);
    //     if (state.EnumerateAllArtifacts().Find(a => a is UsefulScrap) is UsefulScrap us)
    //     {
    //         state.GetCurrentQueue().QueueImmediate(new AHeal
    //         {
    //             healAmount = 1 + us.Amount,
    //             targetPlayer = true,
    //             artifactPulse = us.Key()
    //         });
    //     }
    //     else
    //     {
    //         state.GetCurrentQueue().QueueImmediate(new AHeal
    //         {
    //             healAmount = 1,
    //             targetPlayer = true
    //         });
    //     }
    // }
}