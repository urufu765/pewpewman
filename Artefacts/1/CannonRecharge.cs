using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ])]
public class CannonRecharge : Artifact
{
    public bool ReadyToGive {get; set;}
    public override void OnTurnEnd(State state, Combat combat)
    {
        if (combat.energy > 0)
        {
            ReadyToGive = true;
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (ReadyToGive)
        {
            combat.Queue(
                new AStatus
                {
                    status = Status.stunCharge,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            );
            ReadyToGive = false;
        }
    }

    public override List<Tooltip> GetExtraTooltips()
    {
        return [
            new TTGlossary("status.stunCharge", ["1"])
        ];
    }
}