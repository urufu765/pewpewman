using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicEvade : RelicShield
{
    public override Status GetThing()
    {
        return Status.evade;
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