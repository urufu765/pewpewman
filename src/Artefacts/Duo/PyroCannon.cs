using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ]), DuoArtifactMeta(duoDeck = Deck.eunice)]
public class PyroCannon : Artifact
{
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if (fromPlayer)
        {
            return state.ship.Get(Status.heat);
        }
        return 0;
    }
    public override void OnReceiveArtifact(State state)
    {
        state.ship.heatTrigger -= 1;
    }

    public override void OnRemoveArtifact(State state)
    {
        state.ship.heatTrigger += 1;
    }
}