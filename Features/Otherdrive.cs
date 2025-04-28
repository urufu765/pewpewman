using System.Reflection;
using Weth.External;
using HarmonyLib;
using System.Linq;
using Weth.Artifacts;
using System;

namespace Weth.Features;

public class Otherdriving
{

    public Otherdriving()
    {
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetDmg)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_MDDTP_butDOUBLE))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActualDamage)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_MDDTP_butBEFORE))
        );
    }

    public static void Ship_MDDTP_butBEFORE(ref int __result, State s, bool targetPlayer = false)
    {
        // if (targetPlayer && s.route is Combat c && c is not null && s.EnumerateAllArtifacts().Any(a => a is PyroCannon))
        // {
        //     __result += c.otherShip.Get(Status.heat);
        // }
    }

    public static void Ship_MDDTP_butDOUBLE(ref int __result, State s, bool targetPlayer = false)
    {
        if (!targetPlayer && s.EnumerateAllArtifacts().Any(a => a is PowerCrystals))
        {
            __result += s.ship.Get(Status.shard);
        }
        if (!targetPlayer && s.EnumerateAllArtifacts().Any(a => a is PyroCannon))
        {
            __result += s.ship.Get(Status.heat);
        }
    }
}
