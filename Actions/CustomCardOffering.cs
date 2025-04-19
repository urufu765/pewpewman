namespace Weth.Actions;

public class AWethCardOffering : CardAction
{
    public Card card = null!;
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        return new CardReward
        {
            cards = [card],
            canSkip = false
        };
    }
}