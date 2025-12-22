namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class RelicDrawNextTurn : RelicShield
{
    public override Status GetThing()
    {
        return Status.drawNextTurn;
    }
}