using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicPulsedrive : RelicShield
{
    public override Status GetThing()
    {
        return ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 1);
    }
}