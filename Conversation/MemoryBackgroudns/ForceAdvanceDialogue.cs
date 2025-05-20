using System;
using HarmonyLib;
using Weth;
using Weth.Conversation;
public static class WethForceAdvanceDialogue
{
    private static double breatheTime;
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(Dialogue).GetMethod(nameof(Dialogue.OnInputPhase), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethForceAdvanceDialogue), nameof(ForceDialogueOnScream))
        );
        harmony.Patch(
            original: typeof(Character).GetMethod(nameof(Character.DrawFace), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethForceAdvanceDialogue), nameof(DrawWethCharOverlay))
        );
    }

    /// <summary>
    /// Force-advances the dialogue and doesn't wait for player input
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="g"></param>
    private static void ForceDialogueOnScream(Dialogue __instance, G g)
    {
        if (!__instance.alreadyAdvancedThisFrame && __instance.bg is BGWethVault bwv && bwv.ForceAdvanceDialogue)
        {
            __instance.alreadyAdvancedThisFrame = true;
            __instance.OnPlayerAdvanceInput(g);
        }
    }

    private static void DrawWethCharOverlay(Character __instance, G g, double x, double y, bool flipX, bool mini, bool? isSelected)
    {
        if (__instance.type != ModEntry.WethTheSnep.CharacterType) return;
        if (mini) return;
        if (
            DB.charPanels.TryGetValue(__instance.type, out Spr frame) &&
            WethArtAndFrameSwitcher.GetWethCharIndexFromFrame(frame) is int i &&
            WethArtAndFrameSwitcher.GetWethCharOverlay(i) is Spr overlay &&
            WethArtAndFrameSwitcher.GetWethCharGlow(i) is Spr glow
        )
        {
            breatheTime += g.dt;
            Color c = Colors.white.gain(Mutil.Lerp(0, 0.5, (Math.Sin(breatheTime / 4 * Math.PI) + 1) / 2));
            if (isSelected == false) c.gain(0.4);
            Draw.Sprite(overlay, x, y, flipX);
            Draw.Sprite(glow, x, y, flipX, color: c, blend: BlendMode.Screen);
        }
    }
}