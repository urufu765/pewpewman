using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class PageantRibbon : WethRelicFour
{
    public bool Rotato { get; set; } = false;

    public override int? GetDisplayNumber(State s)
    {
        return null;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            combat.QueueImmediate(new ASpawn
            {
                thing = new Missile
                {
                    yAnimation = 0,
                    missileType = RollForMissile(state.rngActions),
                    targetPlayer = !Special && Rotato
                },
                artifactPulse = Key()
            });
        }
    }

    public override void OnCombatEnd(State state)
    {
        Rotato = !Rotato;
    }

    public MissileType RollForMissile(Rand rng)
    {
        return Mutil.Roll(
            rng.Next(),
            (0.6, MissileType.normal),
            (0.3, MissileType.heavy),
            (0.1, MissileType.seeker)
        );
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("midrow.missile_normal", [2]));
        tips.Add(new TTGlossary("midrow.missile_heavy", [3]));
        tips.Add(new TTGlossary("midrow.missile_seeker", [2]));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class PageantRibbonFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(PageantRibbon);
    
    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("midrow.missile_normal", [2]));
        tips.Add(new TTGlossary("midrow.missile_heavy", [3]));
        tips.Add(new TTGlossary("midrow.missile_seeker", [2]));
        return tips;
    }
}