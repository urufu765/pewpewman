using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Weth.Artifacts;

namespace Weth.Actions;

public class ALoseRelic : CardAction
{
    public required WethRelicFour relic;
    public int amount = 1;
    public override void Begin(G g, State s, Combat c)
    {
        if (s.EnumerateAllArtifacts().Find(a => a == relic) is WethRelicFour wrf)
        {
            if (wrf.Amount > amount)
            {
                wrf.Amount -= amount;
                return;
            }

            ALoseArtifact.DoRemoveArtifact(s, wrf.Key());
        }
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return relic.GetTooltips();
    }
}