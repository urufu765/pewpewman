using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Unreleased ])]
public class SpaceRelics : Artifact
{
    public Dictionary<Status, int> Relics { get; set; } = new Dictionary<Status, int>
    {
        {Status.shield, 0},
        {Status.tempShield, 0},
        {Status.hermes, 0},
        {Status.evade, 0},  // No number
        {Status.autododgeRight, 0},  // No number
        {Status.libra, 0},
        {Status.stunCharge, 0},
        {Status.drawNextTurn, 0},
        {Status.energyFragment, 0},  // No number
        {Status.tempPayback, 0},
        {Status.droneShift, 0},  // No number
        {Status.shard, 0},  // No number
        {Status.boost, 0}
    };
    private Dictionary<Status, Spr> RelicIcons { get; set; } = new Dictionary<Status, Spr>
    {
        {Status.shield, StableSpr.icons_shield},
        {Status.tempShield, StableSpr.icons_tempShield},
        {Status.hermes, StableSpr.icons_hermes},
        {Status.evade, StableSpr.icons_evade},  // No number
        {Status.autododgeRight, StableSpr.icons_autododgeRight},  // No number
        {Status.libra, StableSpr.icons_libra},
        {Status.stunCharge, StableSpr.icons_stunCharge},
        {Status.drawNextTurn, StableSpr.icons_drawNextTurn},
        {Status.energyFragment, StableSpr.icons_energyFragment},  // No number
        {Status.tempPayback, StableSpr.icons_tempPayback},
        {Status.droneShift, StableSpr.icons_droneShift},  // No number
        {Status.shard, StableSpr.icons_shard},  // No number
        {Status.boost, StableSpr.icons_boost}
    };
    public int ObtainPulsedrive {get; set;}

    public void ObtainRelic(Status status)
    {
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
        List<Tooltip> tt = new List<Tooltip>();

        // string thing = "";
        // if (ObtainPulsedrive > 0)
        // {
        //     thing += string.Format(ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "desc"]), $"<c=keyword>{ObtainPulsedrive}</c>") + "\n";
        // }
        // foreach (KeyValuePair<Status, int> relic in Relics)
        // {
        //     if (relic.Value > 0)
        //     {
        //         thing += string.Format(ModEntry.Instance.Localizations.Localize(["status", $"{relic.Key}", "desc"]), $"<c=keyword>{relic.Value}</c>") + "\n";
        //     }
        // }
        // tt.Add(new TTText(thing));
        if (ObtainPulsedrive > 0)
        {
            tt.Add(new TTTTTTGlossary($"showStatus.pulsedrive")
            {
                Title = string.Format(ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "desc"]), $"<c=keyword>{ObtainPulsedrive}</c>"),
                Icon = ModEntry.Instance.PulseStatus.Configuration.Definition.icon,
            });
            tt.Add(new TTTTTTText(" "));
        }
        foreach (KeyValuePair<Status, int> relic in Relics)
        {
            if (relic.Value > 0)
            {
                tt.Add(new TTTTTTGlossary($"showStatus.{relic.Key}")
                {
                    Title = string.Format(ModEntry.Instance.Localizations.Localize(["status", $"{relic.Key}", "desc"]), $"<c=keyword>{relic.Value}</c>"),
                    Icon = RelicIcons[relic.Key],
                });
                tt.Add(new TTTTTTText(" "));
            }
        }
        if (tt.Count > 0)
        {
            tt.Add(new TTDivider());
            if (ObtainPulsedrive > 0)
            {
                tt.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, ObtainPulsedrive));
            }
            foreach (KeyValuePair<Status, int> relic in Relics)
            {
                if (relic.Value > 0)
                {
                    if (relic.Key is Status.evade or Status.autododgeRight or Status.energyFragment or Status.droneShift)
                    {
                        tt.Add(new TTGlossary($"status.{relic.Key}"));
                    }
                    else if (relic.Key is Status.shard)
                    {
                        tt.Add(new TTGlossary($"status.{relic.Key}", [$"{MG.inst.g.state.ship.GetMaxShard()}"]));
                    }
                    else
                    {
                        tt.Add(new TTGlossary($"status.{relic.Key}", [$"{relic.Value}"]));
                    }
                }
            }
        }
        return tt;
    }
}