using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ])]
public class RockPower : Artifact
{
    public override void OnAsteroidIsDestroyed(State state, Combat combat, bool wasPlayer, int worldX)
    {
        if (wasPlayer)
        {
            combat.Queue(
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            );
        }
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> l = StatusMeta.GetTooltips(ModEntry.Instance.PulseStatus.Status, 1);
        l.Insert(0, new TTGlossary("midrow.asteroid"));
        return l;
    }
}