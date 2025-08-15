using System;
using System.Collections.Generic;
using System.Linq;
using Weth.Cards;

namespace Weth.Actions;

public class AVolleySplitshotFromAllCannons : CardAction
{
    public ASplitshot splitshot = null!;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        splitshot.multiCannonVolley = true;
        splitshot.fast = true;
        List<ASplitshot> splitshots = [];
        for (int x = 0; x < s.ship.parts.Count; x++)
        {
            if (s.ship.parts[x].type == PType.cannon && s.ship.parts[x].active)
            {
                splitshot.fromX = x;
                splitshots.Add(Mutil.DeepCopy<ASplitshot>(splitshot));
            }
        }
        c.QueueImmediate(new AJupiterShoot
        {
            attackCopy = ASplitshot.ConvertSplitToAttack(splitshot),
        });
        c.QueueImmediate(splitshots);
    }
}