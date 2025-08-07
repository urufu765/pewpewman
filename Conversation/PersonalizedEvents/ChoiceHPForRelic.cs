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
public static class ChoiceHPForRelic
{
    public static void WoahWhatsThat(State s, ref List<Choice> __result)
    {
        if (s.characters.Any(a => a.type == ModEntry.WethTheSnep.CharacterType) && s.EnumerateAllArtifacts().Any(b => b is TreasureHunter) && __result.Count > 1)
        {
            int hurting = s.GetHardEvents() ? 3 : 4;
            if (s.EnumerateAllArtifacts().Any(b => b is TreasureSeeker))
            {
                hurting--;
            }
            __result[1] = new Choice
            {
                label = string.Format(ModEntry.Instance.Localizations.Localize(["event", "ChoiceHPForRelic_Yes", "desc"]), hurting),
                key = "ChoiceHPForRelic_Yes",
                actions = [
                    new AHurt
                    {
                        hurtAmount = hurting,
                        targetPlayer = true
                    },
                    new AWethSingleArtifactOffering
                    {
                        artifact = new SpaceUrchinFake{ Special = s.EnumerateAllArtifacts().Any(a => a is TreasureSeeker) },
                        canSkip = false
                    }
                ]
            };
        }
    }
}
