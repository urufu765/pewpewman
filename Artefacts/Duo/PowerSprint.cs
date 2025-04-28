using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ], unremovable = true), DuoArtifactMeta(duoDeck = Deck.riggs)]
public class PowerSprint : Artifact
{
    public int OldEvadeAmount {get; set;}
    public bool Depleted {get; set;}
    public override void OnTurnStart(State state, Combat combat)
    {
        OldEvadeAmount = -1984;
        Depleted = false;
    }

    public override void OnReceiveArtifact(State state)
    {
        Depleted = false;
        OldEvadeAmount = -1984;
        if (state.ship.evadeMax is int evado && evado <= 5)
        {
            return;
        }
        state.ship.evadeMax = 5;
    }

    public void DoThing(int evadeAmount)
    {
        OldEvadeAmount = evadeAmount;
    }

    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if(!Depleted && status == Status.evade && mode == AStatusMode.Add && OldEvadeAmount != -1984 && state.ship.evadeMax is int evMax && state.ship.Get(Status.evade) == evMax)
        {
            int amount = statusAmount - (evMax - OldEvadeAmount);
            if (amount <= 0) return;
            combat.QueueImmediate(
                new AStatus{
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = amount,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            );
            Depleted = true;
        }
    }
}

public static class ArtifactPowersprintEvadeOperator
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(AStatus).GetMethod("Begin", AccessTools.all),
            prefix: new HarmonyMethod(typeof(ArtifactPowersprintEvadeOperator), nameof(FindEvade))
        );
    }

    private static void FindEvade(State s)
    {
        int? amount = s.ship?.Get(Status.evade);
        Artifact? artifact = s.EnumerateAllArtifacts().Find(a => a is PowerSprint);
        if (amount is int amt && artifact is PowerSprint ps)
        {
            ps.DoThing(amt);
        }
    }
}
