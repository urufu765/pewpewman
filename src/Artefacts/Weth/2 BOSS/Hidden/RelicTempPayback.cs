namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicTempPayback : RelicShield
{
    public override Status GetThing()
    {
        return Status.tempPayback;
    }
}