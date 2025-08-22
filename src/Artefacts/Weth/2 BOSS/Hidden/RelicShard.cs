using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicShard : RelicShield
{
    public override Status GetThing()
    {
        return Status.shard;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary($"status.{GetThing()}", [$"{MG.inst.g.state.ship.GetMaxShard()}"])
        ];
    }
}