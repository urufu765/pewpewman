using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.EventOnly ])]
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
        if (status == ModEntry.Instance.PulseStatus.Status)
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
                        status = ModEntry.Instance.PulseStatus.Status,
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

        if (ObtainPulsedrive > 0)
        {
            tt.Add(new GlossaryTooltip($"showStatus.pulsedrive")
            {
                Title = ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["status", "pulsedrive", "description"]),
                Icon = ModEntry.Instance.PulseStatus.Configuration.Definition.icon,
            });
        }
        foreach (KeyValuePair<Status, int> relic in Relics)
        {
            if (relic.Value > 0)
            {
                tt.Add(new GlossaryTooltip($"showStatus.{relic.Key}")
                {
                    Title = ModEntry.Instance.Localizations.Localize(["status", $"{relic.Key}", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["status", $"{relic.Key}", "description"]),
                    Icon = RelicIcons[relic.Key],
                });
            }
        }
        return tt;
    }
}