using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SuperDriveCollector : Artifact
{
    public int TurnsShowedOff { get; set; }
    public bool Showoffable { get; set; }
    public override int? GetDisplayNumber(State s)
    {
        return TurnsShowedOff;
    }

    public override void OnCombatEnd(State state)
    {
        TurnsShowedOff = 0;
        Showoffable = false;
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        TurnsShowedOff = 0;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Showoffable = true;
        if (CompletedCollection(state.ship))
        {
            Pulse();
            TurnsShowedOff++;
            Showoffable = false;
        }
    }

    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if (Showoffable)
        {
            Status? includeStatus = null;
            if (mode is AStatusMode.Add or AStatusMode.Set && statusAmount > 0)
            {
                includeStatus = status;
            }

            if (CompletedCollection(state.ship, includeStatus))
            {
                Pulse();
                TurnsShowedOff++;
                Showoffable = false;
            }
        }
    }

    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if (fromPlayer) return TurnsShowedOff;
        return 0;
    }

    public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
    {
        if (!targetPlayer) return TurnsShowedOff;
        return 0;
    }

    public override int ModifySpaceMineDamage(State state, Combat? combat, bool targetPlayer)
    {
        if (!targetPlayer) return TurnsShowedOff;
        return 0;
    }

    /// <summary>
    /// Checks whether the player has a complete collection of Drives
    /// </summary>
    /// <param name="playerShip">The player ship</param>
    /// <param name="includeStatus">Statuses that are about to be added that may not be in player ship yet</param>
    /// <returns></returns>
    private static bool CompletedCollection(Ship playerShip, Status? includeStatus = null)
    {
        bool mini = false, pulse = false, over = false, power = false;
        switch (includeStatus)
        {
            case Status x when x == ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive:
                mini = true;
                break;
            case Status x when x == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive:
                pulse = true;
                break;
            case Status.overdrive:
                over = true;
                break;
            case Status.powerdrive:
                power = true;
                break;
        }

        if (playerShip.Get(ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive) > 0) mini = true;
        if (playerShip.Get(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive) > 0) pulse = true;
        if (playerShip.Get(Status.overdrive) > 0) over = true;
        if (playerShip.Get(Status.powerdrive) > 0) power = true;

        return mini && pulse && over && power;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> l = [];

        l.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive, 1));
        l.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 1));
        l.AddRange([
            new TTGlossary($"status.overdrive", ["1"]),
            new TTGlossary($"status.powerdrive", ["1"])
        ]);
        return l;
    }
}