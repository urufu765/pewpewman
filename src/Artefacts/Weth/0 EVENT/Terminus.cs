using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;

namespace Weth.Artifacts;


public enum Terminus
{
    Pick,
    Active,
    Inactive,
    Reward,
    AltReward
}

public class TheTerminus : Artifact
{
    public int Counter {get; set;}
    public Terminus Mode {get; set;}

    public override int? GetDisplayNumber(State s)
    {
        return Counter;
    }

    public override void OnReceiveArtifact(State state)
    {
        Counter = 0;
        Mode = Terminus.Pick;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return Mode switch
        {
            Terminus.Reward => [new GlossaryTooltip("terminustt.reward")
            {
                Description = ModEntry.Instance.Localizations.Localize(["Weth", "artifact", "Tooltips", "TerminusReward"])
            }],
            Terminus.AltReward => [new GlossaryTooltip("terminustt.altreward")
            {
                Description = ModEntry.Instance.Localizations.Localize(["Weth", "artifact", "Tooltips", "TerminusAltReward"])
            }],
            _ => base.GetExtraTooltips()
        };
    }
}