using FSPRO;
using System.Linq;
using BBB = Weth.API.IArtifactModifyBurnBlisterBaseDamage;

namespace Weth.Actions;

public class ABurnDamage : CardAction
{
    public bool targetPlayer;
    public int? lastHeat;  // Since the burn damage hits *AFTER* the player overheats

    public override void Begin(G g, State s, Combat c)
    {
        timer *= 2;  // ACorrode does it so why shouldn't this?
        Ship ship = targetPlayer ? s.ship : c.otherShip;
        if (ship is null) return;

        int damage = 1;  // Base damage
        foreach (BBB iambbbd in s.EnumerateAllArtifacts().OfType<BBB>())
        {
            damage += iambbbd.ModifyBurnBaseDamage(s, c, targetPlayer);
        }

        // Bonus damage from heat
        damage += lastHeat ?? ship.Get(Status.heat);
        // Maybe change formula to
        // damage += ship.Get(Status.heat) == 0? lastHeat ?? 0 : ship.Get(Status.heat)
        // Or instead of seeing if zero, compare with lastHeat and pick the bigger of the two

        ship.NormalDamage(s, c, damage, null, false);
        Audio.Play(Event.Status_CorrodeHurt);  // Maybe replace with a whoosh
        ship.PulseStatus(ModEntry.Instance.BurnStatus.Status);
    }
}