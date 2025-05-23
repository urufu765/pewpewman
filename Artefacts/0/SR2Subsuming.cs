using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SR2Subsuming : SpaceRelics2
{
    public Dictionary<Status, int> RelicCharges { get; set; } = new Dictionary<Status, int>
    {
        {Status.shield, -1},
        {Status.tempShield, -1},
        {Status.hermes, -1},
        {Status.evade, -1},  // No number
        {Status.autododgeRight, -1},  // No number
        {Status.libra, -1},
        {Status.stunCharge, -1},
        {Status.drawNextTurn, -1},
        {Status.energyFragment, -1},  // No number
        {Status.tempPayback, -1},
        {Status.droneShift, -1},  // No number
        {Status.shard, -1},  // No number
        {Status.boost, -1}
    };
    public int PulsedriveCharge { get; set; } = -1;
    public bool InCombat { get; set; }

    /// <summary>
    /// Apply countdowns
    /// </summary>
    /// <param name="state"></param>
    /// <param name="combat"></param>
    public override void OnCombatStart(State state, Combat combat)
    {
        InCombat = true;
        if (ObtainPulsedrive > 0)
        {
            PulsedriveCharge = ObtainPulsedrive;
        }
        foreach (Status status in RelicCharges.Keys)
        {
            if (Relics[status] > 0)
            {
                RelicCharges[status] = Relics[status];
            }
        }
    }

    /// <summary>
    /// Clear countdowns
    /// </summary>
    /// <param name="state"></param>
    public override void OnCombatEnd(State state)
    {
        InCombat = false;
        PulsedriveCharge = -1;
        foreach (Status status in RelicCharges.Keys)
        {
            RelicCharges[status] = -1;
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (PulsedriveCharge > 0)
        {
            combat.Queue(
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            );
            PulsedriveCharge--;
        }
        foreach (Status status in RelicCharges.Keys)
        {
            if (RelicCharges[status] > 0)
            {
                combat.Queue(
                    new AStatus
                    {
                        status = status,
                        statusAmount = 1,
                        targetPlayer = true,
                        artifactPulse = Key()
                    }
                );
                RelicCharges[status]--;
            }
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        if (InCombat)
        {
            List<Tooltip> tt = [];
            if (PulsedriveCharge >= 0)
            {
                tt.Add(new TTTTTTGlossary($"showStatus.pulsedrive")
                {
                    Title = string.Format(ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "desc"]), $"(<c=keyword>{PulsedriveCharge}</c>)"),
                    Icon = ModEntry.Instance.PulseStatus.Configuration.Definition.icon,
                });
                tt.Add(new TTTTTTText(" "));
            }
            tt.AddRange(GetVanillaStatusIconNNamez(RelicCharges, 0, true));
            if (tt.Count > 0)
            {
                tt.Add(new TTDivider());
                if (PulsedriveCharge >= 0)
                {
                    tt.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 1));
                }
                tt.AddRange(GetVanillaStatusGenericTooltips(RelicCharges, 0, 1));
            }
            return tt;
        }
        else return base.GetExtraTooltips();
    }
}