using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;
using Weth.Features;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common]), DuoArtifactMeta(duoDeck = Deck.eunice)]
public class DoubleEdgedHeat : Artifact
{
    public int Stack { get; set; }

    public override void OnCombatEnd(State state)
    {
        Stack = 0;
    }

    public override int? GetDisplayNumber(State s)
    {
        return Stack;
    }

    public override void AfterPlayerOverheat(State state, Combat combat)
    {
        if (Stack > 0)
        {
            combat.QueueImmediate(new AHurt
            {
                hurtAmount = Stack,
                hurtShieldsFirst = true,
                targetPlayer = true
            });
        }
        Pulse();
        Stack++;
    }

    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if (fromPlayer && card is Card c && (c.GetMeta().deck == Deck.eunice || c.GetMeta().deck == ModEntry.Instance.RoadkillDeck.Deck))
        {
            return Stack;
        }
        return base.ModifyBaseDamage(baseDamage, card, state, combat, fromPlayer);
    }
}