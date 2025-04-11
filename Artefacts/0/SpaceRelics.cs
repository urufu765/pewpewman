using System;
using System.Collections.Generic;
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
        {Status.evade, 0},
        {Status.autododgeRight, 0},
        {Status.libra, 0},
        {Status.stunCharge, 0},
        {Status.drawNextTurn, 0},
        {Status.energyFragment, 0},
        {Status.tempPayback, 0},
        {Status.droneShift, 0},
        {Status.shard, 0},
        {Status.boost, 0}
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

    public override void OnCombatStart(State state, Combat combat)
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

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tt = new List<Tooltip>();

        if (ObtainPulsedrive > 0)
        {
            tt.AddRange(StatusMeta.GetTooltips(ModEntry.Instance.PulseStatus.Status, ObtainPulsedrive));
        }
        foreach (KeyValuePair<Status, int> relic in Relics)
        {
            if (relic.Value > 0)
            {
                if (relic.Key is Status.evade or Status.droneShift or Status.autododgeRight or Status.energyFragment or Status.shard)
                {
                    tt.Add(new TTGlossary($"status.{relic.Key}"));
                    tt.Add(new TTText(ModEntry.Instance.Localizations.Localize(["status", relic.Key.ToString(), "desc"])));
                }
                else
                {
                    tt.Add(new TTGlossary($"status.{relic.Key}", [relic.Value.ToString()]));
                }
            }
        }
        return tt;
    }
}