using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;


public enum JauntState
{
    Pick,
    Hidden,
    Ready,
    SuperReady
}

[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class RelicJaunt : Artifact
{
    public int TotalRelics { get; set; }
    public JauntState Jstate { get; set; }
    private const int Goal = 5;
    //public bool GotDaArtifact {get; set;};
    // private int _lastDamageDealt;
    // public int LastDamageDealt { get {return _lastDamageDealt;} set {_lastDamageDealt = Math.Max(0, value);} }

    public override int? GetDisplayNumber(State s)
    {
        return TotalRelics;
    }

    // public override void OnTurnStart(State state, Combat combat)
    // {
    //     LastDamageDealt = 0;
    // }

    public override void OnReceiveArtifact(State state)
    {
        TotalRelics = 0;
        Jstate = JauntState.Pick;
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
        return Jstate switch
        {
            JauntState.Hidden => ModEntry.Instance.SprArtExcBeyond,
            JauntState.Ready => ModEntry.Instance.SprArtExcReady,
            JauntState.SuperReady => ModEntry.Instance.SprArtExcCounting,
            _ => ModEntry.Instance.SprArtExcPick
            //_ => base.GetSprite()
        };
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        List<int> partRando = [];
        for (int x = 0; x < combat.otherShip.parts.Count; x++) partRando.Add(x);
        partRando.Shuffle();
        foreach (int y in partRando)
        {
            if (combat.otherShip.parts?[y] is not null && combat.otherShip.parts[y].type is not PType.empty)
            {
                ModEntry.Instance.Helper.ModData.SetModData(combat.otherShip.parts[y], "jauntable", true);
                Jstate = JauntState.Hidden;
                break;
            }
        }
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        if (part is not null && ModEntry.Instance.Helper.ModData.TryGetModData(part, "jauntable", out bool b) && b && Jstate == JauntState.Hidden)
        {
            TotalRelics++;
            if (TotalRelics % Goal == 0)
            {
                Jstate = JauntState.SuperReady;
            }
            else
            {
                Jstate = JauntState.Ready;
            }
            // Custom space bone-breaking sfx
            Pulse();
        }
    }

    public override void OnCombatEnd(State state)
    {
        if (Jstate == JauntState.Ready)
        {
            ModEntry.Instance.Logger.LogInformation("Riches GET");
            state.rewardsQueue.Queue(new AArtifactOffering
            {
                amount = 3,
                limitPools = [ArtifactPool.Unreleased],
                limitDeck = ModEntry.Instance.WethDeck.Deck
            });
        }
        if (Jstate == JauntState.SuperReady)
        {
            ModEntry.Instance.Logger.LogInformation("Bonus Riches GET");
            state.rewardsQueue.Queue(new AArtifactOffering
            {
                amount = 3,
                limitPools = [ArtifactPool.Common]
            });
        }
        Jstate = JauntState.Pick;
    }
}