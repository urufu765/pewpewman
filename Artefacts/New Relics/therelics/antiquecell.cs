using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.AntiqueCell)]
public class AntiqueCell : NewWethSpaceRelics
{
    public static void DoOnTurnStartThing(RelicCollection r, State s, Combat c, int n)
    {
        if (c.turn % 3 == 0)
        {
            c.QueueImmediate(new AStatus
            {
                status = Status.energyFragment,
                statusAmount = n,
                targetPlayer = true
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips()?? [];
        tips.Add(new TTGlossary("status.energyFragment", ["1"]));
        return tips;
    }
}