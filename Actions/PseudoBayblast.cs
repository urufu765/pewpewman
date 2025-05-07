namespace Weth.Actions;

public class APseudoBayblast : AAttack
{
    public bool flared;
    public override Icon? GetIcon(State s)
    {
        if (ABayBlastV2.HaveWeGotAnyMissileBays(s))
        {
            return new Icon(this.flared? ModEntry.Instance.SprBayBlastWide : ModEntry.Instance.SprBayBlast, null, Colors.attack, false);
        }
        return new Icon(this.flared? ModEntry.Instance.SprBayBlastWideFail : ModEntry.Instance.SprBayBlastFail, null, Colors.attackFail, false);
    }
}