using System;
using System.Collections.Generic;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SR2Crackling : SpaceRelics2
{
    public override void ObtainRelic(Status status)
    {
        base.ObtainRelic(status);
        // Upgrade relic (if applicable)
        int pulsedriveAmount = ObtainPulsedrive;
        if (Attempt2Upgrade(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, ref pulsedriveAmount) is Status ps)
        {
            ObtainPulsedrive = pulsedriveAmount;
            Relics[ps]++;
        }
        foreach (Status pear in Relics.Keys)
        {
            int amount = Relics[pear];
            if (Attempt2Upgrade(pear, ref amount) is Status s)
            {
                Relics[pear] = amount;
                Relics[s]++;
            }
        }
    }

    private Status? Attempt2Upgrade(Status status, ref int amount)
    {
        if (status == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive && amount >= 3)
        {
            amount -= 3;
            return Status.overdrive;
        }
        switch (status, amount)
        {
            case (Status.shield, >= 3):
                amount -= 3;
                return Status.maxShield;
            case (Status.tempShield, >= 3):
                amount -= 3;
                return Status.perfectShield;
            case (Status.hermes, >= 3):
                amount -= 3;
                return Status.strafe;
            case (Status.autododgeRight, >= 3):
                amount -= 3;
                return Status.ace;
            case (Status.stunCharge, >= 3):
                amount -= 3;
                return Status.stunSource;
            case (Status.drawNextTurn, >= 3):
                amount -= 3;
                return Status.energyNextTurn;
            case (Status.tempPayback, >= 3):
                amount -= 3;
                return Status.payback;
            case (Status.shard, >= 3):
                amount -= 3;
                return Status.quarry;
            case (Status.quarry, >= 3):
                amount -= 3;
                return Status.maxShard;
            case (Status.overdrive, >= 3):
                amount -= 3;
                return Status.powerdrive;
        }

        return null;
    }

}