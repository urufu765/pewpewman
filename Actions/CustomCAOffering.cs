using System.Collections.Generic;
using Weth.Artifacts;

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