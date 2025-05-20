using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

public static class WethArtAndFrameSwitcher
{
    private static HashSet<string> Ending1Saw { get; } = ["RunWinWho_Weth_1"];
    private static HashSet<string> Ending2Saw { get; } = ["RunWinWho_Weth_2"];
    private static HashSet<string> Ending3Saw { get; } = ["RunWinWho_Weth_3"];
    private static List<string> AllMemories { get; } = ["Weth_Memory_1", "Weth_Memory_2", "Weth_Memory_3"];
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(Events).GetMethod(nameof(Events.RunWinWho), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(SwitchTheArt))
        );
        harmony.Patch(
            original: typeof(State).GetMethod(nameof(State.GoToZone), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(SwitchTheFrame))
        );
        harmony.Patch(
            original: typeof(Vault).GetMethod(nameof(Vault.GetVaultMemories), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(SwitchTheFrameInVault))
        );
        harmony.Patch(
            original: typeof(State).GetMethod(nameof(State.Update), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(ReapplyFrameOnStartup))
        );
        harmony.Patch(
            original: typeof(Vault).GetMethod(nameof(Vault.LoadFromVault), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(UseMemoryFrame))
        );
    }

    /// <summary>
    /// Switches the runWinWho full character sprite according to progression and also switches frame for that dialogue
    /// </summary>
    /// <param name="s"></param>
    private static void SwitchTheArt(State s)
    {
        bool editFrame = true;
        if (!BGRunWin.charFullBodySprites.ContainsKey(AmWethDeck)) return;
        if (!DB.charPanels.ContainsKey(ModEntry.WethTheSnep.CharacterType)) editFrame = false;
        //ModEntry.Instance.Logger.LogInformation("Eep!");
        if (Ending2Saw.Fast_AllAreIn(s.storyVars.visitedNodes))
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEndrotend;
            if (editFrame) SetWethCharFrame(3);
        }
        else if (Ending1Saw.Fast_AllAreIn(s.storyVars.visitedNodes))
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEndrot;
            if (editFrame) SetWethCharFrame(2);
        }
        else
        {
            BGRunWin.charFullBodySprites[AmWethDeck] = ModEntry.Instance.WethEnd;
            if (editFrame) SetWethCharFrame(1);
        }
    }

    /// <summary>
    /// Switches the character frame according to the map's dialogue thing.
    /// </summary>
    /// <param name="nextMap"></param>
    private static void SwitchTheFrame(MapBase nextMap)
    {
        // if (!(ModEntry.Instance.Helper.Content.Characters.V2.LookupByCharacterType(ModEntry.WethTheSnep.CharacterType) is Character ch && __instance.characters.Contains(ch)))
        // {
        //     ModEntry.Instance.Logger.LogWarning("Couldn't find Weth!");
        //     return;
        // }
        if (!DB.charPanels.ContainsKey(ModEntry.WethTheSnep.CharacterType))
        {
            ModEntry.Instance.Logger.LogWarning("Why is Weth not in the panel list?");
            return;
        }
        if (nextMap is not null)
        {
            SetWethCharFrame(nextMap.GetZoneDialogueTag() switch
            {
                "zone_first" => 1,
                "zone_lawless" => 2,
                "zone_three" => 3,
                _ => 0
            });
        }
    }

    /// <summary>
    /// Reapplies correct character frame on load, with a few frames for redundancy
    /// </summary>
    /// <param name="__instance"></param>
    private static void ReapplyFrameOnStartup(State __instance)
    {
        if (ModEntry.Instance.WethFrameLoadAllowed)
        {
            ModEntry.Instance.Logger.LogInformation("GITIT");
            if (__instance.route is Dialogue d && AllMemories.Contains(d.ctx.script))
            {
                SetWethCharFrame();
                return;
            }
            if (__instance.map is not null) SwitchTheFrame(__instance.map);
        }
    }

    /// <summary>
    /// Switches the frame according to the runwinwho visited
    /// </summary>
    /// <param name="s"></param>
    private static void SwitchTheFrameInVault(State s)
    {
        if (!DB.charPanels.ContainsKey(ModEntry.WethTheSnep.CharacterType))
        {
            ModEntry.Instance.Logger.LogWarning("Why is Weth not in the panel list?");
            return;
        }
        if (Ending3Saw.Fast_AllAreIn(s.storyVars.visitedNodes)) SetWethCharFrame(3);
        else if (Ending2Saw.Fast_AllAreIn(s.storyVars.visitedNodes)) SetWethCharFrame(2);
        else if (Ending1Saw.Fast_AllAreIn(s.storyVars.visitedNodes)) SetWethCharFrame(1);
        else SetWethCharFrame();
    }

    private static void UseMemoryFrame(string memoryKey)
    {
        if (AllMemories.Contains(memoryKey)) SetWethCharFrame();
    }

    public static Spr GetWethCharFrame(int n = 0)
    {
        return n switch
        {
            3 => ModEntry.Instance.WethFrameC,
            2 => ModEntry.Instance.WethFrameB,
            1 => ModEntry.Instance.WethFrameA,
            _ => ModEntry.Instance.WethFramePast
        };
    }

    public static Spr? GetWethCharOverlay(int n = 0)
    {
        return n switch
        {
            3 => ModEntry.Instance.WethFrameOverlayC,
            2 => ModEntry.Instance.WethFrameOverlayB,
            1 => ModEntry.Instance.WethFrameOverlayA,
            _ => null            
        };
    }
    public static Spr? GetWethCharGlow(int n = 0)
    {
        return n switch
        {
            3 => ModEntry.Instance.WethFrameGlowC,
            2 => ModEntry.Instance.WethFrameGlowB,
            1 => ModEntry.Instance.WethFrameGlowA,
            _ => null            
        };
    }



    public static int? GetWethCharIndexFromFrame(Spr sprite)
    {
        if (sprite == GetWethCharFrame()) return 0;
        else if (sprite == GetWethCharFrame(1)) return 1;
        else if (sprite == GetWethCharFrame(2)) return 2;
        else if (sprite == GetWethCharFrame(3)) return 3;
        return null;
    }

    public static void SetWethCharFrame(int n = 0)
    {
        try
        {
            DB.charPanels[ModEntry.WethTheSnep.CharacterType] = GetWethCharFrame(n);
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "OH NO FRAME DEAD");
        }
    }
}