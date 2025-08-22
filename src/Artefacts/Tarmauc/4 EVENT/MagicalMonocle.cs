using System.Linq;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class MagicalMonocle : Artifact
{
    public bool Depleted { get; set; }
    public int CardsDrawnThisTurn { get; set; }
    public const int DRAW_LIMIT = 5;

    public override int? GetDisplayNumber(State s)
    {
        return CardsDrawnThisTurn;
    }

    public override Spr GetSprite()
    {
        return Depleted ? StableSpr.artifacts_TestArtifact : base.GetSprite();
    }

    public override void OnTurnEnd(State state, Combat combat)
    {
        if (!Depleted && CardsDrawnThisTurn < DRAW_LIMIT)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.drawNextTurn,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = Key()
            });
            Depleted = true;
        }
        CardsDrawnThisTurn = 0;
    }

    public override void OnCombatEnd(State state)
    {
        CardsDrawnThisTurn = 0;
        Depleted = false;
    }

    public override void OnDrawCard(State state, Combat combat, int count)
    {
        CardsDrawnThisTurn += count;
    }
}