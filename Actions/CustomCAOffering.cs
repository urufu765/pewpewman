using System.Collections.Generic;
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