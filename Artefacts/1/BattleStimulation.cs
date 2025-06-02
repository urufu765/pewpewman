using System.Collections.Generic;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class BattleStimulation : Artifact
{
    public bool StimuliDepleted { get; set; }  // Whether the BS has been activated (and can't be activated no more)
    public bool Stimulated { get; set; }  // Whether the BS is active


    public override void OnCombatStart(State state, Combat combat)
    {
        StimuliDepleted = false;
    }

    public override void OnCombatEnd(State state)
    {
        StimuliDepleted = Stimulated = false;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Stimulated = false;
    }

    public override Spr GetSprite()
    {
        if (Stimulated) return ModEntry.Instance.SprArtBattleStimulationStimulated;
        if (StimuliDepleted) return ModEntry.Instance.SprArtBattleStimulationDepleted;
        return base.GetSprite();
    }

    public void OnEnemyLoseHullCustom(Combat combat)
    {
        if (!StimuliDepleted)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = Key()
            });
            Stimulated = StimuliDepleted = true;
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary($"status.overdrive", ["1"])
        ];
    }
}

public static class BattleStimulationHelper
{
    public static void DetectEnemyLoseHull(Ship __instance, State s, Combat c, int amt)
    {
        if (__instance.hull <= 0) return;
        if (amt <= 0) return;
        if (__instance.Get(Status.perfectShield) > 0) return;
        if (!__instance.isPlayerShip && s.EnumerateAllArtifacts().Find(a => a is BattleStimulation) is BattleStimulation b)
        {
            b.OnEnemyLoseHullCustom(c);
        }
    }
}