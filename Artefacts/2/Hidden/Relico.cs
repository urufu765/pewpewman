using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicShield : Artifact
{
    public virtual Status GetThing()
    {
        return Status.shield;
    }

    public override void OnReceiveArtifact(State state)
    {
        if (!state.EnumerateAllArtifacts().Any(x => x is SpaceRelics))
        {
            state.GetCurrentQueue().QueueImmediate(new AAddArtifact
            {
                artifact = new SpaceRelics()
            });
        }
        ArtifactRemoval(state);
    }

    public override void OnRemoveArtifact(State state)
    {
        foreach (Character character in state.characters)
        {
            if (character.deckType == ModEntry.Instance.WethDeck.Deck)
            {
                foreach (Artifact artifact in character.artifacts)
                {
                    if (artifact is SpaceRelics sr)
                    {
                        sr.ObtainRelic(GetThing());
                        ModEntry.Instance.Logger.LogInformation("GotDaRelic!");
                    }
                }
            }
        }    
    }

    public virtual void ArtifactRemoval(State state)
    {
        state.GetCurrentQueue().Queue(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}",
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary($"status.{GetThing()}", ["1"])
        ];
    }
}