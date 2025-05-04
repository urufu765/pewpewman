using System;
using System.Collections.Generic;
using Weth.Actions;
using Weth.API;
using Weth.Cards;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ], unremovable = true), DuoArtifactMeta(duoDeck = Deck.hacker)]
public class HiddenGem : Artifact, IArtifactWethGoodieUncommonRestrictor
{
    public bool DoIImposeGoodieUncommonRestriction()
    {
        return false;
    }

    public bool DoIOverrideGoodieUncommonRestriction()
    {
        return true;
    }

    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new AWethCardOffering
        {
            cards = [
                new CryEnergy{
                    singleUseOverride = false,
                    exhaustOverride = true,
                    exhaustOverrideIsPermanent = true,
                    temporaryOverride = false
                },
                new CryCapacity{
                    singleUseOverride = false,
                    exhaustOverride = true,
                    exhaustOverrideIsPermanent = true,
                    temporaryOverride = false
                },
                new CryFlux{
                    singleUseOverride = false,
                    exhaustOverride = true,
                    exhaustOverrideIsPermanent = true,
                    temporaryOverride = false
                },
            ]
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTCard
        {
            card = new CryPlaceholder
            {
                singleUseOverride = false,
                exhaustOverride = true,
                exhaustOverrideIsPermanent = true,
                temporaryOverride = false
            },
            showCardTraitTooltips = true
        }];
    }

}