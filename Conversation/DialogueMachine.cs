using Weth.Dialogue;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;

namespace Weth;

public static class DialogueMachine
{
    public static void Apply()
    {
        StoryDialogue.Inject();
        EventDialogue.Inject();
        CombatDialogue.Inject();
        CardDialogue.Inject();
        ArtifactDialogue.Inject();
    }


    public static void ApplyInjections()
    {
        try
        {
            if (!ModEntry.Instance.modDialogueInited)
            {
                ModEntry.Instance.modDialogueInited = true;
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Failed to inject dialogue for modded stuff");
        }
    }
}