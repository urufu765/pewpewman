using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Weth.Actions;
using Weth.API;
using Weth.Cards;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common], unremovable = true), DuoArtifactMeta(duoDeck = Deck.hacker)]
public class HiddenGem : Artifact, IArtifactWethGoodieUncommonRestrictor
{
    private static IEnumerable<Type> GemGoodies { get; } = [
        typeof(MechBubble),
        typeof(MechDodge),
        typeof(MechMissile),
        typeof(CryEnergy),
        typeof(CryCapacity),
        typeof(CryFlux)
    ];

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
            cards = GetGoodies([..GemGoodies])
        });
        state.GetCurrentQueue().QueueImmediate(new AWethCardOffering
        {
            cards = GetGoodies([..GemGoodies])
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


    private static List<Card> GetGoodies(params Type[] items)
    {
        List<Card> result = [];
        List<Type> failed = [];
        try
        {
            foreach (Type t in items)
            {
                if (Activator.CreateInstance(t) is Card c)
                {
                    //ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(state, c, ModEntry.Instance.Helper.Content.Cards.SingleUseCardTrait, false, true);
                    c.singleUseOverride = false;
                    c.exhaustOverride = c.exhaustOverrideIsPermanent = true;
                    c.temporaryOverride = false;
                    result.Add(c);
                }
                else failed.Add(t);
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Turning cards into crds failrdiahwerosd");
        }
        finally
        {
            if (failed.Count > 0)
            {
                ModEntry.Instance.Logger.LogWarning(
                    "Failed to create card instances of: [{ListOfFailedCards}]",
                    string.Join(", ", failed.Select(a => a.Name))
                );
            }
        }
        return result;
    }
}