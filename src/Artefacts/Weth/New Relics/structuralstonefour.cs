using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class StructuralStone : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }

    public override void GainStack(State state, bool? special = null)
    {
        base.GainStack(state, special);
        int hullAvailable = Special? state.ship.hullMax:(state.ship.hull - 1);
        int hurtAvailable = state.ship.hull - 1;
        state.GetCurrentQueue().QueueImmediate(new AShipUpgrades
        {
            actions = [
                new AHullMax
                {
                    amount = hullAvailable,
                    targetPlayer = true
                },
                new AHurt
                {
                    hurtAmount = hurtAvailable,
                    cannotKillYou = true,
                    targetPlayer = true
                }
            ],
            artifactPulse = Key()
        });
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class StructuralStoneFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(StructuralStone);
}