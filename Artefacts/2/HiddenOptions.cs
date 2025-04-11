using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class HiddenOptions : Artifact
{
    public virtual int GetArtifactAmount()
    {
        return 4;
    }

    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new AArtifactOffering
        {
            amount = GetArtifactAmount(),
            limitPools = [ ArtifactPool.Boss ]
        });
    }
}