namespace Weth.Actions;

public class AWethCardOffering : CardAction
{
    public Card? card;
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new CardReward
        {
            cards = card is not null ? [card] : CardReward.GetOffering(s, 1, ModEntry.Instance.WethDeck.Deck),
            canSkip = false
        };
    }
}