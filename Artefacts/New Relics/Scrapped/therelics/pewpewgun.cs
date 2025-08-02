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

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.PewPewGun)]
public class PewPewGun : NewWethSpaceRelics
{
    public static void DoOnCombatStartThing(RelicCollection r, State s, Combat c, int n)
    {
        OverwriteRelicData(r, typeof(PewPewGun), new PewPewData { ShotsLeft = n });
        s.ship.Set(ModEntry.Instance.Relic_PewPewGun.Status, n);
    }

    public static void DoOnPlayerPlayCardThing(RelicCollection r, int energyCost, Deck deck, Card card, State s, Combat c, int handPosition, int handCount, int n)
    {
        if (deck == ModEntry.Instance.WethDeck.Deck && GetRelicData(r, typeof(PewPewGun)) is PewPewData p && p.ShotsLeft > 0)
        {
            c.Queue(new AAttack
            {
                damage = card.GetDmg(s, 0),
                fast = true
            });
            OverwriteRelicData(r, typeof(PewPewGun), new PewPewData { ShotsLeft = p.ShotsLeft - 1});
        }
    }
}

public record PewPewData : RelicData
{
    public int ShotsLeft { get; set; } = 0;
}

public class PewPewGunStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public PewPewGunStatus()
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
        if (args.Status == ModEntry.Instance.Relic_PewPewGun.Status) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.Relic_PewPewGun.Status) return null;
        if
        (
            args.State.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc &&
            NewWethSpaceRelics.GetRelicData(rc, typeof(PewPewGun)) is PewPewData ppd
        )
        {
            List<Color> SegmentColours = [];

            for (int i = 0; i < args.Amount; i++)
            {
                SegmentColours.Add(
                    (i < ppd.ShotsLeft)
                    ? new Color("d38018")
                    : ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor
                );
            }

            return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(SegmentColours);
        }

        return null;
    }
}
#endif