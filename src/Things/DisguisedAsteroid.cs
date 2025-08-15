using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nickel;
using Weth.Actions;
using Weth.Artifacts;

namespace Weth.Objects;

public class NinjaAsteroid : Asteroid
{
    public bool bigMine;
    public bool HitBySplitshot { get; set; } = false;


    public override double GetWiggleAmount()
    {
        return 1;
    }

    public override double GetWiggleRate()
    {
        return 2;
    }

    public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
        List<CardAction> actionz = base.GetActionsOnDestroyed(s, c, wasPlayer, worldX) ?? [];
        int mineHurtAmount = bigMine ? 3 : 2;
        if (HitBySplitshot && s.EnumerateAllArtifacts().Find(a => a is SpaceUrchin) is SpaceUrchin su && su.Special)
        {
            mineHurtAmount = 0;
        }
        actionz.Add(new ASpaceMineAttack
        {
            hurtAmount = mineHurtAmount,
            targetPlayer = wasPlayer,
            worldX = worldX
        });
        return actionz;
    }
}