using System;
using System.Collections.Generic;
using System.Linq;
using Weth.Cards;

namespace Weth.Actions;

public class AVolleyBlastFromAllBays2 : CardAction
{
    public ABayBlastV2 bayblast = null!;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        bayblast.multiBayVolley = true;
        bayblast.fast = true;
        List<ABayBlastV2> bayblasts = [];
        for (int x = 0; x < s.ship.parts.Count; x++)
        {
            if (s.ship.parts[x].type == PType.missiles && s.ship.parts[x].active)
            {
                bayblast.fromX = x;
                bayblasts.Add(Mutil.DeepCopy<ABayBlastV2>(bayblast));
            }
        }
        c.QueueImmediate(bayblasts);
    }
}