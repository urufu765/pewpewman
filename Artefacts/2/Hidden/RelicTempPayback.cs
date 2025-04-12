using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicTempPayback : RelicShield
{
    public override Status GetThing()
    {
        return Status.tempPayback;
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
            new TTGlossary($"status.{GetThing()}", ["1"])
        ];
    }
}