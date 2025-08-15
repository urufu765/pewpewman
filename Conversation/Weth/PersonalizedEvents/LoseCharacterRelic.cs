using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Weth.Actions;
using Weth.Artifacts;
using Weth.Cards;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

/// <summary>
/// If treasure hunter is present, replaces the card dump option with give milk
/// </summary>
public static class LoseWethArtifact
{
    public static void OhShitOhFuck(State s, ref List<Choice> __result)
    {
        if (s.characters.Any(a => a.type == ModEntry.WethTheSnep.CharacterType) && s.EnumerateAllArtifacts().Any(b => b is WethRelicFour))
        {
            for (int x = 0; x < __result.Count; x++)
            {
                if (__result[x] is Choice c && c.key == $"LoseCharacterCard_{AmWeth}")
                {
                    if (s.EnumerateAllArtifacts().FindAll(a => a is WethRelicFour).Random(new Rand(s.rngCurrentEvent.seed + 82347501u)) is WethRelicFour workingRelic)
                    {
                        __result[x] = new Choice
                        {
                            label = string.Format(ModEntry.Instance.Localizations.Localize(["Weth", "event", "LoseCharacterRelic_Yes", "desc"]), ModEntry.Instance.WethDeck.Configuration.Definition.color, ModEntry.Instance.Localizations.Localize(["Weth", "artifact", "Unreleased", workingRelic.GetType().Name, "label"])),
                            key = $"LoseCharacterRelic_{AmWeth}",
                            actions =
                            {
                                new ALoseRelic
                                {
                                    amount = 1,
                                    relic = workingRelic,
                                }
                            }
                        };
                    }

                }
            }
        }
    }
}
