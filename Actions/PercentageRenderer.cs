using System.Collections.Generic;
using Nickel;

namespace Weth.Actions;

public class AWethPercentageRenderer : AStatus
{

    public override Icon? GetIcon(State s)
    {
        return new Icon(StableSpr.icons_drawLessNextTurn, statusAmount, Colors.redd);
    }
}