using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ])]
public class HiddenOptions : Artifact
{
    public virtual int GetArtifactAmount()
    {
        return 3;
    }

    public virtual List<ArtifactPool> GetArtifactPools()
    {
        return [ ArtifactPool.Common ];
    }

    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new AArtifactOffering
        {
            amount = GetArtifactAmount(),
            limitPools = [ ArtifactPool.Unreleased],
            limitDeck = ModEntry.Instance.WethDeck.Deck,
        });
        state.GetCurrentQueue().QueueImmediate(new AArtifactOffering
        {
            amount = GetArtifactAmount(),
            limitPools = GetArtifactPools(),
        });

        ArtifactRemoval(state);
    }

    public virtual void ArtifactRemoval(State state)
    {
        state.GetCurrentQueue().Queue(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}",
        });
    }
}