using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;
using Weth.External;

#if false
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
            // TODO: Don't allow duplicates
        }
        else
        {
            randomStuff = [];
        }
        s.ship.Set(ModEntry.Instance.Relic_ShockStack.Status, randomStuff.Count);

        OverwriteRelicData(r, typeof(ShockStack), new ShockData { Increments = randomStuff, MaxShocks = n});
        // TODO: If enemy health = max health, and assigned stack is empty, reroll
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
            sd.MaxShocks = n;

            if (i > 0)
            {
                c.QueueImmediate(new AStatus
                {
                    status = Status.stunCharge,
                    statusAmount = i,
                    targetPlayer = true
                });
                ModEntry.Instance.Logger.LogInformation("WAH!");
                // status blip
            }

            OverwriteRelicData(r, typeof(ShockStack), sd);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips()?? [];
        tips.Add(new TTGlossary("status.stunCharge", ["1"]));
        return tips;
    }

    // TODO: Display the ShockStack ability on top of enemy ship, as yellow health bits
}

public record ShockData : RelicData
{
    public int MaxShocks { get; set; } = 0;
    public List<int> Increments { get; set; } = new();  // TODO: maybe use ModData instead?

}

public class ShockStackStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public ShockStackStatus()
    {
        ModEntry.Instance.KokoroApi.V2.StatusLogic.RegisterHook(this);
        ModEntry.Instance.KokoroApi.V2.StatusRendering.RegisterHook(this);
    }

    /// <summary>
    /// No relic statuses should be affected by timestop nor boost
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public bool? IsAffectedByBoost(IKokoroApi.IV2.IStatusLogicApi.IHook.IIsAffectedByBoostArgs args)
    {
        if (args.Status == ModEntry.Instance.Relic_ShockStack.Status) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.Relic_ShockStack.Status) return null;
        if
        (
            args.State.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc &&
            NewWethSpaceRelics.GetRelicData(rc, typeof(ShockStack)) is ShockData sd
        )
        {
            List<Color> SegmentColours = [];

            for (int i = 0; i < sd.MaxShocks; i++)
            {
                if (i < sd.Increments.Count - 1 || i == sd.Increments.Count - 1 && false)  // or when setting is applied
                {
                    SegmentColours.Add(new Color("c69610"));
                }
                else if (i == sd.Increments.Count - 1)
                {
                    SegmentColours.Add(new Color("ffd723"));
                }
                else if (i < args.Amount)
                {
                    SegmentColours.Add(new Color("635326"));
                }
                else
                {
                    SegmentColours.Add(ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor);
                }
            }

            return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(SegmentColours);
        }

        return null;
    }
}
#endif