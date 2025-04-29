using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;
using Weth.Features;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ]), DuoArtifactMeta(duoDeck = Deck.dizzy)]
public class MadcapCharge : Artifact
{
    public bool Depleted {get; set;}

    public void DoThing(Combat combat)
    {
        if (!Depleted)
        {
            Depleted = true;
            combat.Queue(new AStatus
            {
                status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = Key(),
            });
        }
    }

    public override Spr GetSprite()
    {
        return Depleted? ModEntry.Instance.SprArtMadcapDepleted : base.GetSprite();
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Depleted = false;
    }

    public override void OnCombatEnd(State state)
    {
        Depleted = false;
    }
}

public static class ArtifactMadcapPartOperator
{
    private static bool? partHasIntent;
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(AStunPart).GetMethod("Begin", AccessTools.all),
            prefix: new HarmonyMethod(typeof(ArtifactMadcapPartOperator), nameof(DetectIntent)),
            postfix: new HarmonyMethod(typeof(ArtifactMadcapPartOperator), nameof(DetectChange))
        );
    }

    private static void DetectChange(AStunPart __instance, State s, Combat c)
    {
        if (partHasIntent == true)
        {
            Part? partAtWorldX = c.otherShip?.GetPartAtWorldX(__instance.worldX);
            if (partAtWorldX is not null && partAtWorldX.intent is null)
            {
                Artifact? artifact = s.EnumerateAllArtifacts().Find(a => a is MadcapCharge);
                if (artifact is MadcapCharge mcc)
                {
                    mcc.DoThing(c);
                }
            }
        }
    }

    private static void DetectIntent(AStunPart __instance, Combat c)
    {
        partHasIntent = null;
        Part? partAtWorldX = c.otherShip?.GetPartAtWorldX(__instance.worldX);
        if (partAtWorldX is not null)
        {
            partHasIntent = partAtWorldX.intent is not null;
        }
    }
}
