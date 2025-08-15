using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Weth.Artifacts;

namespace Weth.Actions;

public class AWethCardOffering : CardAction
{
    public List<Card> cards = [];
    public bool canSkip = false;
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new CardReward
        {
            cards = cards,
            canSkip = canSkip
        };
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = [];
        if (cards.Count == 1)
        {
            tooltips.AddRange(
            [
                new TTGlossary("action.addCard", [$"<c=deck>{Loc.T("destination.deck.name")}</c>"]),
                new TTCard
                {
                    card = cards[0],
                }
            ]);
        }
        else if (cards.Count > 1)
        {
            tooltips.Add(
                new TTGlossary("action.cardOfferingForWho", [$"<c=2a767d>{Character.GetDisplayName(ModEntry.Instance.WethDeck.Deck, s)}</c>"])
            );
        }
        return tooltips;
    }
}

public class AWethSpaceRelicOffering : CardAction
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new ArtifactReward
        {
            artifacts = [
                new SpaceRelics2(),
                new SR2Crackling(),
                new SR2Focused(),
                new SR2Subsuming()
            ],
            canSkip = false
        };
    }
}

public class AWethSingleArtifactOffering : CardAction
{
    public required Artifact artifact;
    public bool canSkip = true;
    public bool showTooltips = true;

    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new ArtifactReward
        {
            artifacts = [artifact],
            canSkip = this.canSkip
        };
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        if (showTooltips)
        {
            return artifact.GetTooltips();
        }
        return base.GetTooltips(s);
    }
}


public class AWethRandomSingleCardCycleUpgrade : CardAction
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        try
        {
            bool downgraded = false;
            Card card = s.deck.Where(c => c.GetMeta().upgradesTo.Length > 0).Shuffle(s.rngActions).First();

            Upgrade up = card.upgrade + 1;
            if ((int)up > card.GetMeta().upgradesTo.Length)
            {
                up = Upgrade.None;
                downgraded = true;
            }
            card.upgrade = up;

            return new ShowCards
            {
                messageKey = downgraded ? "cardMisc.toothCardExtraTooltip" : "showcards.upgraded",
                cardIds = [card.uuid]
            };
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Whoops, couldn't randomly pick a card to upgrade");
        }
        return null;
    }
}


public class AWethSelectSingleCardCycleUpgrade : CardAction
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        try
        {
            if (selectedCard is not null)
            {
                bool downgraded = false;
                Upgrade up = selectedCard.upgrade + 1;
                if ((int)up > selectedCard.GetMeta().upgradesTo.Length)
                {
                    up = Upgrade.None;
                    downgraded = true;
                }
                selectedCard.upgrade = up;

                return new ShowCards
                {
                    messageKey = downgraded ? "cardMisc.toothCardExtraTooltip" : "showcards.upgraded",
                    cardIds = [selectedCard.uuid]
                };
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Select card upgrade failed!");
        }
        return null;
    }
}