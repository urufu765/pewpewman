using System;
using System.Collections.Generic;
using System.Linq;
using OneOf.Types;
using Weth.Actions;
using Weth.API;
using Weth.Cards;


namespace Weth.Artifacts;


public enum WethEventState
{
    /// <summary>
    /// For repeatable events with no limits
    /// </summary>
    Repeatable,
    /// <summary>
    /// For events that can only be done once
    /// </summary>
    OneTime,
    /// <summary>
    /// If one time event is active
    /// </summary>
    Active,
    /// <summary>
    /// If one time event's requirements are fulfilled and is ready for reward
    /// </summary>
    Success,
    /// <summary>
    /// If one time event is done and finished
    /// </summary>
    Complete,
    /// <summary>
    /// If one time event's fail condition is met
    /// </summary>
    Failed,
    /// <summary>
    /// If event is not available
    /// </summary>
    Unavailable
}


[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class TreasureHunter : Artifact, IArtifactWethGoodieUncommonRestrictor
{
    public static class WethEvents
    {
        public const string Crystal = "Crystal";
        public const string Duncan = "AsteroidDriller";
        public const string AquaticLifeform = "UnderwaterGuy";
        public const string Stardog = "WideMissiler";
        public const string Sasha = "FootballFoe";
        public const string BuriedRelic = "StoneGuy";
    }
    public int hitDuncanDrills = 0;
    public int noStardogMissiles = 0;
    public int stoneAwakeTurnsTaken = 0;

    public int SuccessfulHits { get; set; }
    public bool Depleted { get; set; }
    public Dictionary<string, WethEventState> SpecialEvents { get; set; } = new Dictionary<string, WethEventState>
    {
        {WethEvents.Crystal, WethEventState.Repeatable},
        {WethEvents.Duncan, WethEventState.OneTime},
        {WethEvents.AquaticLifeform, WethEventState.OneTime},
        {WethEvents.Stardog, WethEventState.OneTime},
        {WethEvents.Sasha, WethEventState.OneTime},
        {WethEvents.BuriedRelic, WethEventState.OneTime}
    };
    public bool isCrystal;
    public string CurrentEvent { get; set; } = "";
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
                    upgrade = GetUpgrade(),
                    betterOdds = GetAdvanced()
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
        EnemyGetHitEventUpdater(state, combat, part);
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        string name = "";
        string type = "";
        if (combat.otherShip?.ai is not null)
        {
            type = combat.otherShip.ai.GetType().Name;
            if (combat.otherShip.ai.character?.type is not null)
            {
                name = combat.otherShip.ai.character.type;
            }
        }
        isCrystal = name.Contains("crystal", StringComparison.CurrentCultureIgnoreCase);
        if (isCrystal)
        {
            CurrentEvent = WethEvents.Crystal;
        }
        else
        {
            if (SpecialEvents.TryGetValue(type, out WethEventState current) && current == WethEventState.OneTime)
            {
                SpecialEvents[type] = WethEventState.Active;
                CurrentEvent = type;
            }
        }
        // if (state?.map?.markers[state.map.currentLocation]?.contents is MapBattle mb)
        // {
        //     isBossNotElite = mb.battleType switch
        //     {
        //         BattleType.Elite => false,
        //         BattleType.Boss => true,
        //         _ => null
        //     };
        // }
    }

    public override void OnCombatEnd(State state)
    {
        SuccessfulHits = 0;
        Depleted = false;
        DoHiddenEvent(state);
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return isCrystal ?
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

    public bool DoIImposeGoodieUncommonRestriction()
    {
        return true;
    }

    public bool DoIOverrideGoodieUncommonRestriction()
    {
        return false;
    }


    #region Event stuff
    public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card)
    {
        if (SpecialEvents[WethEvents.AquaticLifeform] == WethEventState.Active && card is Fear)
        {
            SpecialEvents[WethEvents.AquaticLifeform] = WethEventState.Failed;
        }
    }

    public void EnemyGetHitEventUpdater(State state, Combat combat, Part? part)
    {
        if (SpecialEvents[WethEvents.Duncan] == WethEventState.Active && part?.key is not null && part.key.Contains("drill", StringComparison.CurrentCultureIgnoreCase))
        {
            hitDuncanDrills++;
        }

        if (SpecialEvents[WethEvents.Sasha] == WethEventState.Active && part is not null && part.type != PType.empty)
        {
            SpecialEvents[WethEvents.Sasha] = WethEventState.Failed;
        }
    }

    public override void OnPlayerTakeNormalDamage(State state, Combat combat, int rawAmount, Part? part)
    {
        if (SpecialEvents[WethEvents.Sasha] == WethEventState.Active && part is not null && part.type != PType.empty)
        {
            SpecialEvents[WethEvents.Sasha] = WethEventState.Failed;
        }
    }

    public override void OnTurnEnd(State state, Combat combat)
    {
        if (SpecialEvents[WethEvents.Stardog] == WethEventState.Active)
        {
            if (combat.stuff.Values.Any(a => a is Missile))
            {
                noStardogMissiles = 0;
            }
            else
            {
                noStardogMissiles++;
            }

            if (noStardogMissiles >= 2)
            {
                SpecialEvents[WethEvents.Stardog] = WethEventState.Success;
            }
        }

        if (SpecialEvents[WethEvents.BuriedRelic] == WethEventState.Active && combat.otherShip?.ai is StoneGuy sg && sg.hasBeenWokenUp)
        {
            stoneAwakeTurnsTaken++;
        }
    }


    public virtual void DoHiddenEvent(State state)
    {
        if (SpecialEvents.TryGetValue(CurrentEvent, out WethEventState current))
        {
            if (current is WethEventState.Active)  // Check for end conditions
            {
                CheckActiveEvent();
            }

            if (SpecialEvents[CurrentEvent] is WethEventState.Success)  // doesn't use "current" due to the event state being updated in CheckActiveEvent()
            {
                RewardSuccessfulEvent(state);
            }
            else if (SpecialEvents[CurrentEvent] == WethEventState.Repeatable)
            {
                RewardRecurringEvent(state);
            }
        }
        CurrentEvent = "";
    }


    public void CheckActiveEvent()
    {
        switch (CurrentEvent)
        {
            case WethEvents.Duncan:
                if (hitDuncanDrills >= 7) SpecialEvents[WethEvents.Duncan] = WethEventState.Success;
                else SpecialEvents[WethEvents.Duncan] = WethEventState.Failed;
                break;
            case WethEvents.AquaticLifeform:
                SpecialEvents[WethEvents.AquaticLifeform] = WethEventState.Success;
                break;
            case WethEvents.Stardog:
                SpecialEvents[WethEvents.Stardog] = WethEventState.Failed;
                break;
            case WethEvents.Sasha:
                SpecialEvents[WethEvents.Sasha] = WethEventState.Success;
                break;
            case WethEvents.BuriedRelic:
                if (stoneAwakeTurnsTaken < 5) SpecialEvents[WethEvents.BuriedRelic] = WethEventState.Success;
                else SpecialEvents[WethEvents.BuriedRelic] = WethEventState.Failed;
                break;
        }
    }

    public void RewardSuccessfulEvent(State state)
    {
        switch (CurrentEvent)
        {
            case WethEvents.Duncan:
                state.rewardsQueue.QueueImmediate(
                    new AWethSingleArtifactOffering
                    {
                        artifact = new StubbornDrillFake { Special = GetAdvanced() }
                    }
                );
                break;
            case WethEvents.AquaticLifeform:
                state.rewardsQueue.QueueImmediate(
                    new AWethSingleArtifactOffering
                    {
                        artifact = new DeadFishFake { Special = GetAdvanced() }
                    }
                );
                break;
            case WethEvents.Stardog:
                state.rewardsQueue.QueueImmediate(
                    new AWethSingleArtifactOffering
                    {
                        artifact = new PageantRibbonFake { Special = GetAdvanced() }
                    }
                );
                break;
            case WethEvents.Sasha:
                state.rewardsQueue.QueueImmediate(
                    new AWethSingleArtifactOffering
                    {
                        artifact = new AstroGrassFake { Special = GetAdvanced() }
                    }
                );
                break;
            case WethEvents.BuriedRelic:
                state.rewardsQueue.QueueImmediate(
                    new AWethSingleArtifactOffering
                    {
                        artifact = new StructuralStoneFake { Special = GetAdvanced() }
                    }
                );
                break;
        }
        SpecialEvents[CurrentEvent] = WethEventState.Complete;
    }

    public void RewardRecurringEvent(State state)
    {
        switch (CurrentEvent)
        {
            case WethEvents.Crystal:
                state.rewardsQueue.QueueImmediate(
                    new AWethCardOffering
                    {
                        cards = [new CryShield { discount = GetAdvanced() ? -1 : 0 }]
                    }
                );
                break;
        }
    }
    #endregion
}