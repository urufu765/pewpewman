using System;
using System.Collections.Generic;
using System.Linq;
using Weth.Cards;

namespace Weth.Actions;

public class AVolleyBlastFromAllBays : CardAction
{
    public ABayBlast bayblast = null!;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        bayblast.multiBayVolley = true;
        bayblast.fast = true;
        List<ABayBlast> bayblasts = [];
        for (int x = 0; x < s.ship.parts.Count; x++)
        {
            if (s.ship.parts[x].type == PType.missiles && s.ship.parts[x].active)
            {
                bayblast.fromX = x;
                bayblasts.Add(Mutil.DeepCopy<ABayBlast>(bayblast));
            }
        }
        c.QueueImmediate(bayblasts);
    }
}