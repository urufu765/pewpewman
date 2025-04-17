using System;
using System.Collections.Generic;
using Weth.Cards;

namespace Weth.Actions;

/// <summary>
/// An action that gives a random goodie card
/// </summary>
public class AGiveGoodieLikeAGoodBoy : CardAction
{
    public bool fromArtifact;
    public string artifactKey = "";
    public Upgrade upgrade;

    private static readonly List<Type> CrystalOfferings = [
        typeof(CryAhtack),
        typeof(CryDuhfend),
        typeof(CryDodge),
        typeof(CryEnergy),
        typeof(CryEvade),
        typeof(CryFlux),
        typeof(CrySwap)
    ];
    private static readonly List<Type> MechOfferings = [
        typeof(MechAhtack),
        typeof(MechDuhfend),
        typeof(MechEvade),
        typeof(MechMine),
        typeof(MechMissile),
        typeof(MechStun),
        typeof(MechSwap)
    ];

    
    public override void Begin(G g, State s, Combat c)
    {
        Random rng = new Random();
        timer = 0.0;
        string name = "";
        if (c.otherShip?.ai?.character?.type is not null)
        {
            name = c.otherShip.ai.character.type;
        }
        List<Type> offerings = name.ToLower().Contains("crystal") ? CrystalOfferings : MechOfferings;
        
        foreach (Card card in c.discard)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon || card.GetMeta().rarity == Rarity.rare))
            {
                offerings.Remove(card.GetType());
            }
        }
        foreach (Card card in c.hand)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon || card.GetMeta().rarity == Rarity.rare))
            {
                offerings.Remove(card.GetType());
            }
        }
        foreach (Card card in c.exhausted)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon || card.GetMeta().rarity == Rarity.rare))
            {
                offerings.Remove(card.GetType());
            }
        }
        Card cd = (Card)Activator.CreateInstance(offerings[rng.Next(offerings.Count)])!;
        cd.upgrade = upgrade;
        // shove card into deck
        if (fromArtifact)
        {
            c.Queue(
                new AAddCard
                {
                    card = cd,
                    destination = CardDestination.Hand,
                    artifactPulse = artifactKey
                }
            );
        }
        else
        {
            c.Queue(
                new AAddCard
                {
                    card = cd,
                    destination = CardDestination.Deck
                }
            );
        }
    }
}