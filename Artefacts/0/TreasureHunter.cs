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
    public bool Depleted { get; set; }
    public bool isCrystal;
    /// <summary>
    /// Null = normal, False = Elite, True = Boss
    /// </summary>
    public bool? isBossNotElite;

    public override int? GetDisplayNumber(State s)
    {
        return SuccessfulHits;
    }

    public virtual int GetHitsRequired()
    {
        return 10;
    }

    /// <summary>
    /// Either allow just Boss rewards, or both Boss and Elite rewards
    /// </summary>
    /// <returns></returns>
    public virtual bool GetAdvanced()
    {
        return false;
    }

    public virtual string GetArtifactKey()
    {
        return Key();
    }

    public virtual bool CanBeDepleted()
    {
        return true;
    }

    public virtual Upgrade GetUpgrade()
    {
        return Upgrade.None;
    }

    public override Spr GetSprite()
    {
        return SuccessfulHits >= GetHitsRequired() ? ModEntry.Instance.SprArtTHDepleted : base.GetSprite();
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        if (SuccessfulHits < GetHitsRequired()) SuccessfulHits++;
        if (SuccessfulHits >= GetHitsRequired() && !Depleted)
        {
            combat.QueueImmediate(
                new AGiveGoodieLikeAGoodBoy
                {
                    fromArtifact = true,
                    artifactKey = GetArtifactKey(),
                    upgrade = GetUpgrade()
                }
            );
            if (CanBeDepleted())
            {
                Depleted = true;
            }
            else
            {
                SuccessfulHits = 0;
            }
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
        if (state?.map?.markers[state.map.currentLocation]?.contents is MapBattle mb)
        {
            isBossNotElite = mb.battleType switch
            {
                BattleType.Elite => false,
                BattleType.Boss => true,
                _ => null
            };
        }
    }

    public override void OnCombatEnd(State state)
    {
        SuccessfulHits = 0;
        Depleted = false;
        if (isBossNotElite is not null && (isBossNotElite.Value || GetAdvanced()))
        {
            state.rewardsQueue.QueueImmediate(
                new AWethCardOffering
                {
                    card = isCrystal? new CryShield() : new MechHull()
                }
            );
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return isCrystal? 
            [
                new TTCard
                {
                    card = new CryPlaceholder
                    {
                        upgrade = GetUpgrade()
                    },
                    showCardTraitTooltips = true
                }] : 
            [
                new TTCard
                {
                    card = new MechPlaceholder
                    {
                        upgrade = GetUpgrade()
                    },
                    showCardTraitTooltips = true
                }
            ];
    }
}