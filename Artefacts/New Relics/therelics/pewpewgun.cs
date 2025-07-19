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

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.PewPewGun)]
public class PewPewGun : NewWethSpaceRelics
{
    public static void DoOnCombatStartThing(RelicCollection r, State s, Combat c, int n)
    {
        OverwriteRelicData(r, typeof(PewPewGun), new PewPewData { ShotsLeft = n });
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
            OverwriteRelicData(r, typeof(PewPewGun), new PewPewData { ShotsLeft = p.ShotsLeft - 1 });
        }
    }
}

public record PewPewData : RelicData
{
    public int ShotsLeft { get; set; } = 0;  // TODO: Convert this into status and use that instead
}