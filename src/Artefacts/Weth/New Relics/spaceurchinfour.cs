using System;
using System.Collections.Generic;
using Weth.Objects;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class SpaceUrchin : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }


    public override int ModifySpaceMineDamage(State state, Combat? combat, bool targetPlayer)
    {
        if (targetPlayer) return -1;
        return 0;
    }


    public override StuffBase ReplaceSpawnedThing(State state, Combat combat, StuffBase thing, bool spawnedByPlayer)
    {
        if (spawnedByPlayer && thing is Asteroid)
        {
            Pulse();
            return new NinjaAsteroid
            {
                yAnimation = thing.yAnimation,
                bubbleShield = thing.bubbleShield,
                fromPlayer = thing.fromPlayer,
                targetPlayer = thing.targetPlayer,
                pulse = thing.pulse,
                hilight = thing.hilight,
                x = thing.x,
                xLerped = thing.xLerped,
                age = thing.age,
                isHitting = thing.isHitting,
                bigMine = Special
            };
        }
        return thing;
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("midrow.asteroid", []));
        // if (Special)
        // {
        //     tips.Add(new TTGlossary("midrow.spaceMineBig", [3]));
        // }
        // else
        // {
        //     tips.Add(new TTGlossary("midrow.spaceMine", [2]));
        // }
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class SpaceUrchinFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(SpaceUrchin);
    
    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("midrow.asteroid", []));
        // if (Special)
        // {
        //     tips.Add(new TTGlossary("midrow.spaceMineBig", [3]));
        // }
        // else
        // {
        //     tips.Add(new TTGlossary("midrow.spaceMine", [2]));
        // }
        return tips;
    }
}