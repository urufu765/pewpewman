using System.Linq;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class TacticalGoggles : Artifact
{
    public bool CanTactical { get; set; }
    public bool DemoMode { get; set; }
    public override void OnCombatStart(State state, Combat combat)
    {
        if (state.ship.hull == state.ship.hullMax)
        {
            CanTactical = true;
        }
        else
        {
            CanTactical = false;
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        DemoMode = false;
    }

    public override void OnCombatEnd(State state)
    {
        DemoMode = true;
    }

    public override Spr GetSprite()
    {
        return CanTactical || DemoMode ? base.GetSprite() : StableSpr.artifacts_TestArtifact;
    }

    public override void OnPlayerLoseHull(State state, Combat combat, int amount)
    {
        if (amount > 0 && CanTactical)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = Key()
            });
            CanTactical = false;
        }
    }
}