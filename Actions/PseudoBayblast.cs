namespace Weth.Actions;

public class APseudoBayblast : AAttack
{
    public bool flared;
    public override Icon? GetIcon(State s)
    {
        if (ABayBlastV2.HaveWeGotAnyMissileBays(s))
        {
            return new Icon(this.flared? ModEntry.Instance.SprBayBlastFlared : ModEntry.Instance.SprBayBlast, damage, Colors.cheevoGold, false);
        }
        return new Icon(ModEntry.Instance.SprBayBlastGeneralFail, damage, Colors.attackFail, false);
    }
}