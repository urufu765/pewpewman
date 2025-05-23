using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SpaceRelics2 : Artifact
{
    public Dictionary<Status, int> Relics { get; set; } = new Dictionary<Status, int>
    {
        { Status.overdrive, 0 },  // Pulsedrive Upgrade
        { Status.powerdrive, 0 },  // Overdrive Upgrade
        { Status.shield, 0 },
        { Status.maxShield, 0 },  // Shield Upgrade
        { Status.tempShield, 0 },
        { Status.perfectShield, 0 },  // Tempshield Upgrade, No number
        { Status.hermes, 0 },
        { Status.strafe, 0 },  // Hermes Upgrade
        { Status.evade, 0 },  // No number
        { Status.autododgeRight, 0 },  // No number
        { Status.ace, 0 },  // AutododgeRight Upgrade
        { Status.libra, 0 },
        { Status.stunCharge, 0 },
        { Status.stunSource, 0 },  // Stuncharge Upgrade
        { Status.drawNextTurn, 0 },
        { Status.energyNextTurn, 0 },  // Drawnextturn Upgrade
        { Status.energyFragment, 0 },  // No number
        { Status.tempPayback, 0 },
        { Status.payback, 0 },  // temppayback Upgrade
        { Status.droneShift, 0 },  // No number
        { Status.shard, 0 },  // strange number
        { Status.quarry, 0 },  // shard Upgrade
        { Status.maxShard, 0 },  // quarry Upgrade
        { Status.boost, 0 }
    };
    private static Dictionary<Status, Spr> RelicIcons { get; set; } = new Dictionary<Status, Spr>
    {
        { Status.overdrive, StableSpr.icons_overdrive },
        { Status.powerdrive, StableSpr.icons_powerdrive },
        { Status.shield, StableSpr.icons_shield },
        { Status.maxShield, StableSpr.icons_maxShield },
        { Status.tempShield, StableSpr.icons_tempShield },
        { Status.perfectShield, StableSpr.icons_perfectShield },
        { Status.hermes, StableSpr.icons_hermes },
        { Status.strafe, StableSpr.icons_strafe },
        { Status.evade, StableSpr.icons_evade },  // No number
        { Status.autododgeRight, StableSpr.icons_autododgeRight },  // No number
        { Status.ace, StableSpr.icons_ace },
        { Status.libra, StableSpr.icons_libra },
        { Status.stunCharge, StableSpr.icons_stunCharge },
        { Status.stunSource, StableSpr.icons_stunSource },
        { Status.drawNextTurn, StableSpr.icons_drawNextTurn },
        { Status.energyNextTurn, StableSpr.icons_energyNextTurn },
        { Status.energyFragment, StableSpr.icons_energyFragment },  // No number
        { Status.tempPayback, StableSpr.icons_tempPayback },
        { Status.payback, StableSpr.icons_payback },
        { Status.droneShift, StableSpr.icons_droneShift },  // No number
        { Status.shard, StableSpr.icons_shard },  // No number
        { Status.quarry, StableSpr.icons_quarry },
        { Status.maxShard, StableSpr.icons_maxShard },
        { Status.boost, StableSpr.icons_boost }
    };
    public int ObtainPulsedrive { get; set; }

    public virtual void ObtainRelic(Status status)
    {
        // Add relic
        if (status == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive)
        {
            ObtainPulsedrive++;
        }
        else if (Relics.ContainsKey(status))
        {
            Relics[status]++;
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            if (ObtainPulsedrive > 0)
            {
                combat.Queue(
                    new AStatus
                    {
                        status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                        statusAmount = ObtainPulsedrive,
                        targetPlayer = true,
                        artifactPulse = Key()
                    }
                );
            }
            foreach (KeyValuePair<Status, int> relic in Relics)
            {
                if (relic.Value > 0)
                {
                    combat.Queue(
                        new AStatus
                        {
                            status = relic.Key,
                            statusAmount = relic.Value,
                            targetPlayer = true,
                            artifactPulse = Key()
                        }
                    );
                }
            }
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tt = [];
        if (ObtainPulsedrive > 0)
        {
            tt.Add(new TTTTTTGlossary($"showStatus.pulsedrive")
            {
                Title = string.Format(ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "desc"]), $"<c=keyword>{ObtainPulsedrive}</c>"),
                Icon = ModEntry.Instance.PulseStatus.Configuration.Definition.icon,
            });
            tt.Add(new TTTTTTText(" "));
        }
        tt.AddRange(GetVanillaStatusIconNNamez(Relics));
        if (tt.Count > 0)
        {
            tt.Add(new TTDivider());
            if (ObtainPulsedrive > 0)
            {
                tt.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, ObtainPulsedrive));
            }
            tt.AddRange(GetVanillaStatusGenericTooltips(Relics));
        }
        return tt;
    }


    public List<Tooltip> GetVanillaStatusIconNNamez(Dictionary<Status, int> relics, int minAmountToShow = 1, bool bracketing = false)
    {
        List<Tooltip> tt = [];
        foreach (KeyValuePair<Status, int> relic in relics)
        {
            if (relic.Value >= minAmountToShow)
            {
                tt.Add(new TTTTTTGlossary($"showStatus.{relic.Key}")
                {
                    Title = string.Format(ModEntry.Instance.Localizations.Localize(["status", $"{relic.Key}", "desc"]), bracketing? $"(<c=keyword>{relic.Value}</c>)" : $"<c=keyword>{relic.Value}</c>"),
                    //Icon = RelicIcons[relic.Key],
                    Icon = TTGlossary.TryGetIcon(relic.Key.Key()),
                });
                tt.Add(new TTTTTTText(" "));
            }
        }
        return tt;
    }

    public static List<Tooltip> GetVanillaStatusGenericTooltips(Dictionary<Status, int> relics, int minAmountToShow = 1, int? fixDisplayAmount = null)
    {
        List<Tooltip> tt = [];
        foreach (KeyValuePair<Status, int> relic in relics)
        {
            if (relic.Value >= minAmountToShow)
            {
                if (relic.Key is Status.perfectShield or Status.evade or Status.autododgeRight or Status.energyFragment or Status.droneShift)
                {
                    tt.Add(new TTGlossary($"status.{relic.Key}"));
                }
                else if (relic.Key is Status.shard)
                {
                    tt.Add(new TTGlossary($"status.{relic.Key}", [$"{MG.inst.g.state.ship.GetMaxShard()}"]));
                }
                else
                {
                    tt.Add(new TTGlossary($"status.{relic.Key}", [$"{fixDisplayAmount??relic.Value}"]));
                }
            }
        }
        return tt;
    }
}