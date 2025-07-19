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

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.ShockStack)]
public class ShockStack : NewWethSpaceRelics
{
    public static void DoOnCombatStartThing(RelicCollection r, State s, Combat c, int n)
    {
        List<int> randomStuff;

        if (c.otherShip is not null && c.otherShip.hullMax > 1)
        {
            List<int> values = [.. Enumerable.Range(0, n)
                .Select(_ => (int)Math.Floor(s.rngActions.Next() * (c.otherShip.hullMax - 2)) + 1)];
            ModEntry.Instance.Logger.LogInformation(string.Join(", ", values));
            randomStuff = values;
        }
        else
        {
            randomStuff = [];
        }
        OverwriteRelicData(r, typeof(ShockStack), new ShockData { Increments = randomStuff });
    }

    public static void DoOnEnemyGetHit(RelicCollection r, State s, Combat c, Part? part, int n)
    {
        if (GetRelicData(r, typeof(ShockStack)) is ShockData sd && sd.Increments.Count > 0 && c.otherShip is not null)
        {
            int i = 0;
            Stack<int> stacker = new(sd.Increments.OrderBy(x => x));

            while (stacker.TryPeek(out int x) && c.otherShip.hull <= x)
            {
                stacker.Pop();
                i++;
            }
            sd.Increments = [.. stacker];

            if (i > 0)
            {
                c.QueueImmediate(new AStatus
                {
                    status = Status.stunCharge,
                    statusAmount = i,
                    targetPlayer = true
                });
            }

            OverwriteRelicData(r, typeof(ShockStack), sd);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = [];
        tips.Add(new TTGlossary("status.stunCharge", ["1"]));
        return tips;
    }
}

public record ShockData : RelicData
{
    public List<int> Increments { get; set; } = new();  // TODO: maybe use ModData instead?

}