using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ]), DuoArtifactMeta(duoDeck = Deck.peri)]
public class ResidualShot : Artifact
{
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (deck == ModEntry.Instance.WethDeck.Deck || deck is Deck.peri)
        {
            combat.Queue(
                new AAttack
                {
                    damage = card.GetDmg(state, 0),
                    artifactPulse = Key()
                }
            );
        }
    }
}