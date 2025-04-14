namespace Weth.Actions;

public class APseudoAttack : AAttack
{
    public override Icon? GetIcon(State s)
    {
        if (DoWeHaveCannonsThough(s))
        {
            return new Icon(this.piercing? ModEntry.Instance.SprSplitshotPiercing : ModEntry.Instance.SprSplitshot, damage, Colors.redd, false);
        }
        return new Icon(this.piercing? ModEntry.Instance.SprSplitshotPiercingFail : ModEntry.Instance.SprSplitshotFail, damage, Colors.attackFail, false);
    }
}