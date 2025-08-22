using System.Reflection;
using HarmonyLib;
using Shockah.Kokoro;
using Weth.Actions;
using StatusLogic = Shockah.Kokoro.IKokoroApi.IV2.IStatusLogicApi;
using BBB = Weth.API.IArtifactModifyBurnBlisterBaseDamage;
using System.Linq;

namespace Weth.Features;

public class RoadkillBurnBlister : StatusLogic.IHook
{
    public const int BURN_MAX = 3;  // Maybe put this in the interface too
    private static Status Burn => ModEntry.Instance.BurnStatus.Status;
    private static Status Blister => ModEntry.Instance.BlisterStatus.Status;
    public RoadkillBurnBlister()
    {
        ModEntry.Instance.KokoroApi.V2.StatusLogic.RegisterHook(this);

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnAfterTurn)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(CorrodeButBurn))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.NormalDamage)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(IncreaseNormalDamage))
        );
    }

    public bool HandleStatusTurnAutoStep(StatusLogic.IHook.IHandleStatusTurnAutoStepArgs args)
    {
        if (args.Status != Burn && args.Status != Blister) return false;
        if (args.Timing != StatusLogic.StatusTurnTriggerTiming.TurnStart) return false;
        if (args.Amount > 0) args.Amount--;
        return false;
    }

    public int ModifyStatusChange(StatusLogic.IHook.IModifyStatusChangeArgs args)
    {
        if (args.Status == Burn)
        {
            if (args.NewAmount < args.OldAmount) return args.NewAmount;
            if (args.NewAmount > BURN_MAX)
            {
                int extra = args.NewAmount - BURN_MAX;
                args.Ship.Add(Blister, extra);
                return BURN_MAX;
            }
        }
        return args.NewAmount;
    }

    public static void CorrodeButBurn(Ship __instance, Combat c)
    {
        if (__instance.Get(Burn) > 0)
        {
            c.QueueImmediate(new ABurnDamage
            {
                lastHeat = __instance.Get(Status.heat),
                targetPlayer = __instance.isPlayerShip
            });
        }
    }

    /// <summary>
    /// Adds damage to NormalDamage instead of GetDmg because I don't want the damage to be shown in any card renders.
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <param name="incomingDamage"></param>
    public static void IncreaseNormalDamage(Ship __instance, State s, Combat c, ref int incomingDamage)
    {
        if (__instance.Get(Blister) > 0)
        {
            int damage = 1 + __instance.Get(Status.heat);
            foreach (BBB iambbbd in s.EnumerateAllArtifacts().OfType<BBB>())
            {
                damage += iambbbd.ModifyBlisterBaseDamage(s, c, __instance.isPlayerShip);
            }
            incomingDamage += damage;
        }
    }
}