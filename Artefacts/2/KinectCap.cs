using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Boss])]
public class KineticCap : Artifact
{
    public int TurnsPassed { get; set; } = 0;
    public const int TURNS_REQUIRED = 5;
    public const int START_AMOUNT = 1;
    public const int MORE_AMOUNT = 1;


    public override int? GetDisplayNumber(State s)
    {
        return TurnsPassed;
    }


    public override void OnTurnStart(State state, Combat combat)
    {
        TurnsPassed++;
        if (combat.turn == 1)  // Start of combat flux giver
        {
            int giveAmount = START_AMOUNT;
            if (TurnsPassed >= TURNS_REQUIRED)  // Combine with turn based flux giver if condition satisfied
            {
                giveAmount += MORE_AMOUNT;
                TurnsPassed = 0;
            }
            combat.Queue(new AStatus
            {
                status = Status.libra,
                statusAmount = giveAmount,
                targetPlayer = true,
                artifactPulse = Key(),
                dialogueSelector = ".WethPutsOnCap"
            });
        }

        // Mid-combat flux giver
        if (TurnsPassed >= TURNS_REQUIRED)
        {
            TurnsPassed = 0;
            combat.Queue(new AStatus
            {
                status = Status.libra,
                statusAmount = MORE_AMOUNT,
                targetPlayer = true,
                artifactPulse = Key(),
                dialogueSelector = ".WethPutsOnAnotherCap"
            });
        }
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary($"status.libra", ["1"])
        ];
    }
}