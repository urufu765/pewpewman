namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicTempShield : RelicShield
{
    public override Status GetThing()
    {
        return Status.tempShield;
    }
}