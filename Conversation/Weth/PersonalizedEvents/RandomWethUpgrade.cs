using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Weth.Actions;
using Weth.Artifacts;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;


/// <summary>
/// Tries to replace the foreign card giver with a relic, but doesn't work since you can't give character specific relics to ship.
/// </summary>
public static class RandomWethRandomUpgradeAOrB
{
    public static void AddAnotherOption(State s, ref List<Choice> __result)
    {
        if (s.EnumerateAllArtifacts().Any(a => a is TreasureHunter))
        {
            if (s.EnumerateAllArtifacts().Any(a => ModEntry.NewRegularRelicCounterparts.ContainsKey(a.GetType())))
            {
                // Offer to increase two random relic stacks
                __result.Add(new Choice
                {
                    label = ModEntry.Instance.Localizations.Localize(["Weth", "event", "UpgradeRandomAOrB_WethRelics", "desc"]),
                    key = "UpgradeRandomAOrB_After",
                    actions = {
                        new AGainRelicsRandom
                        {
                            count = 2
                        }
                    }
                });
            }
            else
            {
                // Offer a special relic
                __result.Add(new Choice
                {
                    label = ModEntry.Instance.Localizations.Localize(["Weth", "event", "UpgradeRandomAOrB_WethSpecial", "desc"]),
                    key = s.EnumerateAllArtifacts().Any(a => a is TreasureSeeker)? "UpgradeRandomAOrBSpecial_After" : "UpgradeRandomAOrBUnique_After",
                    actions = {
                        new AWethSingleArtifactOffering
                        {
                            artifact = new HeraldNihilityFake{ Special = s.EnumerateAllArtifacts().Any(a => a is TreasureSeeker) },
                            canSkip = false
                        }
                    }
                });
            }
        }
    }
}
