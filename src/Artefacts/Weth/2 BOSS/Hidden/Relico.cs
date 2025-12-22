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
        if (!state.EnumerateAllArtifacts().Any(x => x is SpaceRelics2))
        {
            state.GetCurrentQueue().QueueImmediate(new AWethSpaceRelicOffering());
        }
        ArtifactRemoval(state);
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}",
        });
    }

    public override void OnRemoveArtifact(State state)
    {
        foreach (Artifact artifact in state.EnumerateAllArtifacts())
        {
            if (artifact is SpaceRelics2 sr)
            {
                sr.ObtainRelic(GetThing());
                //ModEntry.Instance.Logger.LogInformation("GotDaRelic!");
                return;
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