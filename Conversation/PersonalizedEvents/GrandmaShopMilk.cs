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
public static class WethGrandmaShop
{
    public static void GrandmaGivesWethAMilkSoda(State s, ref List<Choice> __result)
    {
        if (s.characters.Any(a => a.type == ModEntry.WethTheSnep.CharacterType) && s.EnumerateAllArtifacts().Any(b => b is TreasureHunter) && __result.Count > 1)
        {
            __result[1] = new Choice
            {
                label = ModEntry.Instance.Localizations.Localize(["event", "GrandmaShop_Weth", "desc"]),
                key = "GrandmaShop_Weth",
                actions = [
                    new AWethCardOffering
                    {
                        cards = [new NewMilkSoda()]
                    }
                ]
            };
        }
    }
}
