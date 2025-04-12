using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicPulsedrive : RelicShield
{
    public override Status GetThing()
    {
        return ModEntry.Instance.PulseStatus.Status;
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
        return StatusMeta.GetTooltips(ModEntry.Instance.PulseStatus.Status, 1);
    }
}