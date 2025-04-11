using System;
using System.Collections.Generic;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.EventOnly ])]
public class TreasureHunter : Artifact
{
    public int SuccessfulHits {get; set;}
    public bool isCrystal;

    public override int? GetDisplayNumber(State s)
    {
        return SuccessfulHits;
    }

    public virtual int GetHitsRequired()
    {
        return 10;
    }

    public virtual bool GetAdvanced()
    {
        return false;
    }

    public virtual string GetArtifactKey()
    {
        return Key();
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        SuccessfulHits++;
        if (SuccessfulHits >= GetHitsRequired())
        {
            combat.QueueImmediate(
                new AGiveGoodieLikeAGoodBoy
                {
                    advancedArtifact = GetAdvanced(),
                    artifactKey = GetArtifactKey(),
                }
            );
            SuccessfulHits = 0;
        }
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        string name = "";
        if (combat.otherShip?.ai?.character?.type is not null)
        {
            name = combat.otherShip.ai.character.type;
        }
        isCrystal = name.Contains("crystal", StringComparison.CurrentCultureIgnoreCase);
    }

    public override void OnCombatEnd(State state)
    {
        SuccessfulHits = 0;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return isCrystal? 
            [
                new TTCard
                {
                    card = new CryPlaceholder
                    {
                        temporaryOverride = GetAdvanced()? null : true
                    },
                    showCardTraitTooltips = true
                }] : 
            [
                new TTCard
                {
                    card = new MechPlaceholder
                    {
                        temporaryOverride = GetAdvanced()? null : true
                    },
                    showCardTraitTooltips = true
                }
            ];
    }
}