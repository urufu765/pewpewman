using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Boss])]
public class AlPoToCa : Artifact
{
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            combat.Queue(new AAddCard
            {
                card = new FullCommitment(),
                destination = CardDestination.Hand
            });
        }
    }

    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (card is FullCommitment) Pulse();
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTCard
            {
                card = new FullCommitment(),
                showCardTraitTooltips = true
            }
        ];
    }
}