using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;


public enum ExcursionState
{
    Pick,
    Counting,
    Ready,
    Beyond
}

[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class ArtifactExcursion : Artifact
{
    public int TotalHits { get; set; }
    public ExcursionState Exstate { get; set; }
    public int Stage { get; set; }
    private List<int> Goals {get;} = [30, 60, 100];
    public int LastGoal {get; set;} = 100;
    //public bool GotDaArtifact {get; set;};
    // private int _lastDamageDealt;
    // public int LastDamageDealt { get {return _lastDamageDealt;} set {_lastDamageDealt = Math.Max(0, value);} }

    public override int? GetDisplayNumber(State s)
    {
        return TotalHits;
    }

    // public override void OnTurnStart(State state, Combat combat)
    // {
    //     LastDamageDealt = 0;
    // }

    public override void OnReceiveArtifact(State state)
    {
        TotalHits = 0;
        Stage = 0;
        Exstate = ExcursionState.Counting;
    }

    // public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    // {
    //     ModEntry.Instance.Logger.LogInformation("PlayerPlayedCard!");
        
    //     if ()
    //     {
    //         ModEntry.Instance.Logger.LogInformation($"PlayerPlayedCard! DamageDealtTurn: {state.storyVars.damageDealtToEnemyThisTurn}");
    //     }
    // }

    public override Spr GetSprite()
    {
        return Exstate switch
        {
            ExcursionState.Ready => ModEntry.Instance.SprArtExcReady,
            ExcursionState.Beyond => ModEntry.Instance.SprArtExcBeyond,
            ExcursionState.Counting => ModEntry.Instance.SprArtExcCounting,
            _ => ModEntry.Instance.SprArtExcPick
        };
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        if (Exstate == ExcursionState.Counting)
        {
            if (GetDamageDealt(combat, part) > 0)
            {
                TotalHits++;
            }
            if (TotalHits >= Goals[Stage])
            {
                Exstate = ExcursionState.Ready;
                Pulse();
            }
        }
        else if (Exstate == ExcursionState.Beyond)
        {
            if (GetDamageDealt(combat, part) > 0)
            {
                TotalHits++;
            }
            if (TotalHits%20 == 0 && TotalHits > LastGoal)
            {
                LastGoal = TotalHits;
                Exstate = ExcursionState.Ready;
                Pulse();
            }
        }
    }

    private static int GetDamageDealt(Combat combat, Part? part)
    {
        int damage = 0;
        bool piercing = false;
        if (combat.currentCardAction is AAttack aa)
        {
            damage = aa.damage;
            piercing = aa.piercing;
        }
        if (part is not null && part.GetDamageModifier() == PDamMod.armor && !piercing)
        {
            damage--;
        }
        return damage;
    }

    public override void OnCombatEnd(State state)
    {
        if (Exstate == ExcursionState.Ready)
        {
            ModEntry.Instance.Logger.LogInformation("Money GET");
            state.rewardsQueue.Queue(
                Stage switch 
                {
                    0 => new AArtifactOffering
                    {
                        amount = 2,
                        limitPools = [ ArtifactPool.Common ]
                    },
                    1 => new AArtifactOffering
                    {
                        amount = 3,
                        limitPools = [ ArtifactPool.Common ]
                    },
                    2 => new AArtifactOffering
                    {
                        amount = 2,
                        limitPools = [ ArtifactPool.Boss ]
                    },
                    _ => new AArtifactOffering
                    {
                        amount = 3,
                        limitPools = [ ArtifactPool.Unreleased ],
                        limitDeck = ModEntry.Instance.WethDeck.Deck
                    }
                }
            );
            Stage++;
            if (Stage >= Goals.Count)
            {
                Exstate = ExcursionState.Beyond;
            }
            else
            {
                Exstate = ExcursionState.Counting;
            }
        }
    }
}