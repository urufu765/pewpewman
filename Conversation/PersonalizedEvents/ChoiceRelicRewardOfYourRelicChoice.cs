using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Weth.Artifacts;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;


/// <summary>
/// If treasure hunter is present, replace the card choices with relics
/// </summary>
public static class ChoiceRelicRewardOfYourRelicChoice
{
    public static void ReplaceCardRewardWithRelic(State s, ref List<Choice> __result)
    {
        if (s.EnumerateAllArtifacts().Any(a => a is TreasureHunter))
        {
            for (int x = 0; x < __result.Count; x++)
            {
                if (__result[x] is Choice c && c.key == $"ChoiceCardRewardOfYourColorChoice_{AmWeth}")
                {
                    int offeringAmount = s.GetHardEvents() ? 2 : 3;
                    __result[x] = new Choice
                    {
                        label = string.Format(ModEntry.Instance.Localizations.Localize(["event", "ChoiceRelicRewardOfYourRelicChoice_Yes", "desc"]), ModEntry.Instance.WethDeck.Configuration.Definition.color, Character.GetDisplayName(AmWethDeck, s).ToUpperInvariant(), offeringAmount),
                        key = $"ChoiceCardRewardOfYourColorChoice_{AmWeth}",
                        actions =
                        {
                            new AArtifactOffering
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
}
