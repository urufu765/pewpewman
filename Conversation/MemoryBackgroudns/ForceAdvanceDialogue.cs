using HarmonyLib;
using Weth.Dialogue;
public static class WethForceAdvanceDialogue
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(Dialogue).GetMethod(nameof(Dialogue.OnInputPhase), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethForceAdvanceDialogue), nameof(ForceDialogueOnScream))
        );
    }

    private static void ForceDialogueOnScream(Dialogue __instance, G g)
    {
        if (!__instance.alreadyAdvancedThisFrame && __instance.bg is BGWethVault bwv && bwv.ForceAdvanceDialogue)
        {
            __instance.alreadyAdvancedThisFrame = true;
            __instance.OnPlayerAdvanceInput(g);
        }
    }
}