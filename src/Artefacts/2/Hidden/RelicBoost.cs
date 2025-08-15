namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicBoost : RelicShield
{
    public override Status GetThing()
    {
        return Status.boost;
    }
}