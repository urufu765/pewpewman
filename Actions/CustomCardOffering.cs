using System.Collections.Generic;

namespace Weth.Actions;

public class AWethCardOffering : CardAction
{
    public List<Card> cards = null!;
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
}