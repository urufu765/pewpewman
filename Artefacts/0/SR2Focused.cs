using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SR2Focused : SpaceRelics2
{
    public List<Status> ObtainedRelics { get; set; } = [];
    public const int RELICLIMIT = 3;
    public bool AtMax => ObtainedRelics.Count >= RELICLIMIT;
    public static Dictionary<Status, Type> RelicDic { get; set; } = new Dictionary<Status, Type>
    {
        {Status.shield, typeof(RelicShield)},
        {Status.tempShield, typeof(RelicTempShield)},
        {Status.hermes, typeof(RelicHermes)},
        {Status.evade, typeof(RelicEvade)},  // No number
        {Status.autododgeRight, typeof(RelicAutododgeRight)},  // No number
        {Status.libra, typeof(RelicFlux)},
        {Status.stunCharge, typeof(RelicStunCharge)},
        {Status.drawNextTurn, typeof(RelicDrawNextTurn)},
        {Status.energyFragment, typeof(RelicEnergyFragment)},  // No number
        {Status.tempPayback, typeof(RelicTempPayback)},
        {Status.droneShift, typeof(RelicDroneShift)},  // No number
        {Status.shard, typeof(RelicShard)},  // No number
        {Status.boost, typeof(RelicBoost)}
    };


    public override void ObtainRelic(Status status)
    {
        if (AtMax && !ObtainedRelics.Contains(status))
        {
            base.ObtainRelic(ObtainedRelics[^1]);
            return;
        }
        base.ObtainRelic(status);
        if (!ObtainedRelics.Contains(status)) ObtainedRelics.Add(status);
    }
}