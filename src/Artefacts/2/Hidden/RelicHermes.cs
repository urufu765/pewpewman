namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicHermes : RelicShield
{
    public override Status GetThing()
    {
        return Status.hermes;
    }
}