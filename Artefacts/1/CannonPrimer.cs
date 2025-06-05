using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class CannonPrimer : Artifact
{
    /// <summary>
    /// Prevent a single attack from priming twice
    /// </summary>
    public bool PrimerReady { get; set; }

    /// <summary>
    /// Only let the first attack work
    /// </summary>
    public int AttackN { get; set; }

    /// <summary>
    /// For display purposes only (the sprite)
    /// </summary>
    public bool DontUseDisplayMode { get; set; }

    private bool Primable => PrimerReady && AttackN <= 1;
    private bool PrimableVisual => PrimerReady && AttackN == 0;

    public override Spr GetSprite()
    {
        if (!DontUseDisplayMode) return base.GetSprite();
        return PrimableVisual ? base.GetSprite() : ModEntry.Instance.SprArtCannonPrimerDepleted;
    }

    public override void OnReceiveArtifact(State state)
    {
        DontUseDisplayMode = true;
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        PrimerReady = true;
        AttackN = 0;
    }

    public override void OnCombatEnd(State state)
    {
        PrimerReady = true;
        AttackN = 0;
    }

    public override void OnPlayerAttack(State state, Combat combat)
    {
        AttackN++;
    }

    public override void OnEnemyDodgePlayerAttack(State state, Combat combat)
    {
        if (Primable && combat.currentCardAction is AAttack aAttack)
        {
            PrimerReady = false;
            combat.QueueImmediate(new AStatus
            {
                status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive,
                statusAmount = aAttack.damage * 2,
                targetPlayer = true,
                artifactPulse = Key(),
                dialogueSelector = ".cannonPrimedWeth"
            });
        }
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        return StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive, 1);
    }
}