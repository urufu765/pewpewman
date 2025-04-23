using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

public static class ChoiceRelicRewardOfYourRelicChoice
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(Events).GetMethod("ChoiceCardRewardOfYourColorChoice", AccessTools.all),
            postfix: new HarmonyMethod(typeof(ChoiceRelicRewardOfYourRelicChoice), nameof(ReplaceCardRewardWithRelic))
        );
    }

    private static void ReplaceCardRewardWithRelic(State s, ref List<Choice> __result)
    {
        for (int x = 0; x < __result.Count; x++)
        {
            if (__result[x] is Choice c && c.key == $"ChoiceCardRewardOfYourColorChoice_{AmWeth}")
            {
                int offeringAmount = s.GetHardEvents()? 2 : 3;
                __result[x] = new Choice{
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
