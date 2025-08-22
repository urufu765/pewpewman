using System.Linq;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class SoothingMask : Artifact
{
    public const int HULL_LIMIT = 3;
    public override void OnCombatEnd(State state)
    {
        if (state.ship.hull < HULL_LIMIT)
        {
            state.rewardsQueue.QueueImmediate(new AHeal
            {
                healAmount = 1,
                targetPlayer = true,
                artifactPulse = Key()
            });
        }
    }
}