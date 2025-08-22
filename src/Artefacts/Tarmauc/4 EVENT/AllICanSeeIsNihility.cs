using System.Linq;
using Weth.API;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class VisionsNihility : Artifact, IArtifactModifyBurnBlisterBaseDamage
{
    public int Stacks { get; set; }
    public bool GoalReached { get; set; }
    public int HullGained { get; set; }

    public int ModifyBlisterBaseDamage(State state, Combat combat, bool targetPlayer)
    {
        return (!targetPlayer && !GoalReached) ? Stacks : 0;
    }

    public int ModifyBurnBaseDamage(State state, Combat combat, bool targetPlayer)
    {
        return (!targetPlayer && !GoalReached) ? Stacks : 0;
    }

    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        return (fromPlayer && !GoalReached) ? Stacks : base.ModifyBaseDamage(baseDamage, card, state, combat, fromPlayer);
    }

    public override void OnCombatEnd(State state)
    {
        if (!GoalReached)
        {
            Stacks++;
            state.rewardsQueue.QueueImmediate(new AShipUpgrades
            {
                actions = [
                    new AHullMax
                    {
                        amount = 1,
                        targetPlayer = true
                    },
                    new AHeal
                    {
                        healAmount = 1,
                        targetPlayer = true
                    }
                ],
                artifactPulse = Key()
            });
            HullGained++;
        }
    }

    public void OnGoalReached(State state, Combat combat)
    {
        GoalReached = true;
        combat.QueueImmediate(new AHullMax
        {
            amount = -HullGained,
            targetPlayer = true,
            dialogueSelector = ".roadKillFinisher"
        });
    }

    public override int? GetDisplayNumber(State s)
    {
        return GoalReached ? null : Stacks;
    }
    
    public override Spr GetSprite()
    {
        return GoalReached ? StableSpr.artifacts_TestArtifact : base.GetSprite();
    }
}