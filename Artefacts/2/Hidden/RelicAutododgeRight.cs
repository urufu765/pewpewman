using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicAutododgeRight : RelicShield
{
    public override Status GetThing()
    {
        return Status.autododgeRight;
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
            new TTGlossary($"status.{GetThing()}")
        ];
    }
}