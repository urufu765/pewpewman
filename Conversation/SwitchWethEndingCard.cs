using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

public static class WethEndingArtSwitcher
{
    private static HashSet<string> Ending1Saw {get;} = ["RunWinWho_Weth_1"];
    private static HashSet<string> Ending2Saw {get;} = ["RunWinWho_Weth_2"];
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(Events).GetMethod(nameof(Events.RunWinWho), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethEndingArtSwitcher), nameof(SwitchTheArt))
        );
    }

    private static void SwitchTheArt(State s)
    {
        if (!BGRunWin.charFullBodySprites.ContainsKey(AmWethDeck)) return;
        //ModEntry.Instance.Logger.LogInformation("Eep!");
        if (Ending2Saw.Fast_AllAreIn(s.storyVars.visitedNodes))
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEndrotend;
        }
        else if (Ending1Saw.Fast_AllAreIn(s.storyVars.visitedNodes))
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEndrot;
        }
        else
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEnd;
        }
    }
}