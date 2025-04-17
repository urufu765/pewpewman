using System;
using System.Collections.Generic;
using System.Linq;
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
    public int amount = 1;
    public bool ignoreUncommonRestriction;

    private static readonly List<Type> CrystalOfferings = [
        typeof(CryAhtack),
        typeof(CryDuhfend),
        typeof(CryDodge),
        typeof(CrySwap)
    ];
    private static readonly List<Type> CrystalUncommonOfferings = [
        typeof(CryEnergy),
        typeof(CryEvade),
        typeof(CryFlux)
    ];
    private static readonly List<Type> MechOfferings = [
        typeof(MechAhtack),
        typeof(MechDuhfend),
        typeof(MechEvade),
        typeof(MechSwap)
    ];
    private static readonly List<Type> MechUncommonOfferings = [
        typeof(MechMine),
        typeof(MechMissile),
        typeof(MechStun),
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
        for (int x = 0; x < amount; x++)
        {
            List<Type> offerings = GetPossibleOffering(c, name.ToLower().Contains("crystal"), ignoreUncommonRestriction);
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

    private static List<Type> GetPossibleOffering(Combat c, bool isCrystal, bool ignore = false)
    {
        List<Type> offerings = isCrystal ? CrystalOfferings : MechOfferings;
        bool hasUncommon = false;

        foreach (Card card in c.discard)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                hasUncommon = true;
                goto skipCheck;
            }
        }
        foreach (Card card in c.hand)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                hasUncommon = true;
                goto skipCheck;
            }
        }
        foreach (Card card in c.exhausted)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon || card.GetMeta().rarity == Rarity.rare))
            {
                hasUncommon = true;
            }
        }
    skipCheck:
        if (!hasUncommon)
        {
            offerings = offerings.Concat(isCrystal ? CrystalUncommonOfferings : MechUncommonOfferings).ToList();
        }
        return offerings;
    }
}