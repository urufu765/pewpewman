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
    public bool betterOdds;
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
        typeof(MechBubble),
        typeof(MechMissile),
        typeof(MechDodge),
    ];


    public override void Begin(G g, State s, Combat c)
    {
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
        bool uncommonOffered = HasUncommon(s, c);
        for (int x = 0; x < amount; x++)
        {
            bool rollUncommon = RolledUncommon(s.rngCardOfferingsMidcombat, betterOdds);
            // Restrict uncommon only if there already is one, or if player is receiving multiple at a time.
            if (restrictUncommon && rollUncommon)
            {
                if (uncommonOffered)
                {
                    rollUncommon = false;
                }
                if(!uncommonOffered && !asAnOffering)
                {
                    uncommonOffered = true;
                }
            }
            List<Type> offerings = GetOfferings(name.ToLower().Contains("crystal"), rollUncommon);
            Card cd = (Card)Activator.CreateInstance(offerings.Random(s.rngCardOfferingsMidcombat))!;
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

    private static List<Type> GetOfferings(bool isCrystal, bool giveUncommon)
    {
        if (giveUncommon)
        {
            return isCrystal ? CrystalUncommonOfferings : MechUncommonOfferings;
        }
        else
        {
            return isCrystal ? CrystalOfferings : MechOfferings;
        }
    }

    private static bool RolledUncommon(Rand rng, bool betterOdds)
    {
        return Mutil.Roll(rng.Next(), (betterOdds? 0.67:0.75, false), (betterOdds?0.33:0.25, true));
    }

    private static bool HasUncommon(State s, Combat c)
    {
        foreach (Card card in c.discard)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                return true;
            }
        }
        foreach (Card card in c.hand)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                return true;
            }
        }
        foreach (Card card in c.exhausted)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                return true;
            }
        }
        foreach (Card card in s.deck)
        {
            if (card.GetMeta().deck == ModEntry.Instance.GoodieDeck.Deck && (card.GetMeta().rarity == Rarity.uncommon))
            {
                return true;
            }
        }
        return false;
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