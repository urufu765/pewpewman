using Weth.API;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class DoubleBlind : Artifact
{
    public bool Left { get; set; } = false;
    public bool Right { get; set; } = false;
    public int LastPosition { get; set; }

    public override void OnCombatEnd(State state)
    {
        Left = Right = false;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Left = Right = false;
        LastPosition = state.ship.x;
    }
}


public static class DoubleBlindHelper
{
    public static void CheckMovementAndAct(State s, Combat c)  // BeginOrRouteHelper postfix
    {
        if (s.route is Combat && s.ship.hull > 0 && s.EnumerateAllArtifacts().Find(a => a is DoubleBlind) is DoubleBlind db)
        {
            if (s.ship.x < db.LastPosition && !db.Left)
            {
                db.LastPosition = s.ship.x;
                db.Left = true;
                c.QueueImmediate(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true,
                    hurtShieldsFirst = true,
                    artifactPulse = db.Key()
                });
            }
            else if (s.ship.x > db.LastPosition && !db.Right)
            {
                db.LastPosition = s.ship.x;
                db.Right = true;
                c.QueueImmediate(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = 2,
                    targetPlayer = true,
                    artifactPulse = db.Key()
                });
            }
        }
    }
}