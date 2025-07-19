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

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.DogCharm)]
public class DogCharm : NewWethSpaceRelics
{
    public static void DoOnTurnStartThing(RelicCollection r, State s, Combat c, int n)
    {
        if (c.turn == 1)
        {
            c.QueueImmediate(new AStatus
            {
                status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive,
                statusAmount = n,
                targetPlayer = false
            });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips()?? [];
        tips.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Underdrive, 1));
        return tips;
    }
}