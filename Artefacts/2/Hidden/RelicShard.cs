using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicShard : RelicShield
{
    public override Status GetThing()
    {
        return Status.shard;
    }

    public override void ArtifactRemoval(State state)
    {
        state.GetCurrentQueue().Queue(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}",
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary($"status.{GetThing()}", [$"{MG.inst.g.state.ship.GetMaxShard()}"])
        ];
    }
}