namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicFlux : RelicShield
{
    public override Status GetThing()
    {
        return Status.libra;
    }
}