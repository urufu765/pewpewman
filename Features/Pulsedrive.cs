using System.Reflection;
using Weth.External;
using HarmonyLib;
using Shockah.Kokoro;

namespace Weth.Features;

public class Pulsedriving : IKokoroApi.IV2.IStatusLogicApi.IHook
{

    public Pulsedriving()
    {
        ModEntry.Instance.KokoroApi.V2.StatusLogic.RegisterHook(this);

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetDmg)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_MDDTP_butDOUBLE))
        );
    }


    public static void Ship_MDDTP_butDOUBLE(ref int __result, State s, bool targetPlayer = false)
    {
        if (s.route is Combat c && c is not null)
        {
            Ship ship = targetPlayer ? c.otherShip : s.ship;
            __result += ship.Get(ModEntry.Instance.PulseStatus.Status);
        }
    }

    public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
    {
        if (args.Status != ModEntry.Instance.PulseStatus.Status) return false;
        if (args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart) return false;
        args.Amount = 0;
        return false;
    }
}
