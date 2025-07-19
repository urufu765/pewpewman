using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased]), RelicMeta(theRelic = WethRelics.UsefulScrap)]
public class UsefulScrap : NewWethSpaceRelics
{
    public override void OnReceiveArtifact(State state)
    {
        if (state.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection r)
        {
            state.GetCurrentQueue().QueueImmediate(new AHeal
            {
                healAmount = 1 + r.Relics[WethRelics.UsefulScrap],
                targetPlayer = true
            });
        }
        else
        {
            state.GetCurrentQueue().QueueImmediate(new AHeal
            {
                healAmount = 1,
                targetPlayer = true
            });
        }
        base.OnReceiveArtifact(state);
    }
    // Make sure to prefix GetTooltips to apply the stupid quantity vals thing
}