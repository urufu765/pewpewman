using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;


/// <summary>
/// Tries to replace the foreign card giver with a relic, but doesn't work since you can't give character specific relics to ship.
/// TODO: Replace with foreign RoadKill offering
/// </summary>
public static class ForeignRelicOffering
{
    public static void ReplaceCardRewardWithRelic(State s, ref List<Choice> __result)
    {
        for (int x = 0; x < __result.Count; x++)
        {
            if (__result[x] is Choice c && c.label is not null && c.label.Contains("WETH", StringComparison.InvariantCultureIgnoreCase))
            {
                int offeringAmount = s.GetHardEvents() ? 2 : 3;
                __result[x] = new Choice
                {
                    label = string.Format(ModEntry.Instance.Localizations.Localize(["Weth", "event", "ForeignRelicOffering_Yes", "desc"]), ModEntry.Instance.WethDeck.Configuration.Definition.color, Character.GetDisplayName(AmWethDeck, s).ToUpperInvariant(), offeringAmount),
                    key = "ForeignCardOffering_After",
                    actions =
                    {
                        new AArtifactOffering  // Doesn't work
                        {
                            amount = offeringAmount,
                            limitDeck = AmWethDeck,
                            limitPools = [ArtifactPool.Unreleased],
                            canSkip = false,
                        }
                    }
                };
            }
        }
    }
}
