
using FSPRO;
using Nickel;

namespace Weth.Actions;

public class ASodaBoom : CardAction
{
    public double? shake;
    public int? damage;
    public int? shakenAmount;
    public override void Begin(G g, State s, Combat c)
    {
        timer = 0;
        if (damage is int d && d > 0)
        {
            c.QueueImmediate(new AHurt
            {
                hurtAmount = d,
                targetPlayer = true
            });
        }
        else
        {
            Audio.Play(Event.Hits_HitHurt);
            PlayerScreenDamage.OneShot();
        }
        c.QueueImmediate(new AStatus
        {
            status = Status.drawLessNextTurn,
            targetPlayer = true,
            statusAmount = shakenAmount ?? 1
        });
        s.shake += shake ?? 3;
    }
}