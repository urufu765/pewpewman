using System;
using System.Collections.Generic;
using System.Linq;
using Weth.API;
using Weth.Artifacts;
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
    public CardDestination? destination = null;
    public bool asAnOffering = false;

    public static readonly List<Type> CrystalOfferings = [
        typeof(CryAhtack),
        typeof(CryDuhfend),
        typeof(CryCapacity),
        typeof(CrySwap)
    ];
    public static readonly List<Type> CrystalUncommonOfferings = [
        typeof(CryEnergy),
        typeof(CryEvade),
        typeof(CryFlux)
    ];
    public static readonly List<Type> MechOfferings = [
        typeof(MechAhtack),
        typeof(MechDuhfend),
        typeof(MechEvade),
        typeof(MechSwap)
    ];
    public static readonly List<Type> MechUncommonOfferings = [
        typeof(MechMine),
        typeof(MechMissile),
        typeof(MechDodge),
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
        bool restrictUncommon = false;
        bool overruleRestriction = false;
        foreach (Artifact artifact in s.EnumerateAllArtifacts())
        {
            if (artifact is IArtifactWethGoodieUncommonRestrictor iwgur)
            {
                if (iwgur.DoIImposeGoodieUncommonRestriction()) restrictUncommon = true;
                if (iwgur.DoIOverrideGoodieUncommonRestriction()) overruleRestriction = true;
            }
        }

        if (ignoreUncommonRestriction || overruleRestriction) restrictUncommon = false;
        List<Card> cardz = [];
        for (int x = 0; x < amount; x++)
        {
            List<Type> offerings = GetPossibleOffering(c, name.ToLower().Contains("crystal"), restrictUncommon);
            offerings.Shuffle(s.rngCardOfferingsMidcombat);
            Card cd = (Card)Activator.CreateInstance(offerings[0])!;
            cd.upgrade = upgrade;
            // shove card into deck
            if (asAnOffering)
            {
                cardz.Add(cd);
            }
            else
            {
                if (fromArtifact)
                {
                    c.Queue(
                        new AAddCard
                        {
                            card = cd,
                            destination = destination??CardDestination.Hand,
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
                            destination = destination??CardDestination.Deck
                        }
                    );
                }
            }
        }
        if (asAnOffering)
        {
            c.Queue(new AWethCardOffering
            {
                cards = cardz,
                artifactPulse = fromArtifact? artifactKey : null,
                canSkip = true,
            });
        }
    }

    private static List<Type> GetPossibleOffering(Combat c, bool isCrystal, bool restrictUncommon)
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
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                hasUncommon = true;
            }
        }
    skipCheck:
        if (!hasUncommon || !restrictUncommon)
        {
            offerings = offerings.Concat(isCrystal ? CrystalUncommonOfferings : MechUncommonOfferings).ToList();
        }
        return offerings;
    }
}