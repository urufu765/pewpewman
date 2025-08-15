using System.Collections.Generic;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class CalculatedWhiff : Artifact
{
    public int Whiffs { get; set; }
    public const int MAX_WHIFFS = 2;
    private readonly bool limitless = true;  // In case a balance change is needed
    private bool CanWhiff => limitless || Whiffs < MAX_WHIFFS;

    public override int? GetDisplayNumber(State s)
    {
        return limitless? null : Whiffs;
    }

    public override Spr GetSprite()
    {
        return CanWhiff? base.GetSprite() : ModEntry.Instance.SprArtCalculatedWhiffDepleted;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Whiffs = 0;
    }

    public override void OnCombatEnd(State state)
    {
        Whiffs = 0;
    }

    public override void OnEnemyDodgePlayerAttackByOneTile(State state, Combat combat)
    {
        if (CanWhiff)
        {
            combat.QueueImmediate(new AStatus
            {
                targetPlayer = true,
                statusAmount = 1,
                status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                artifactPulse = Key()
            });
            Whiffs++;
        }
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        return StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 1);
    }
}