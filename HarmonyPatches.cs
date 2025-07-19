using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Weth.Artifacts;
using Weth.Cards;
using Weth.External;
using Weth.Features;
using System.Reflection;
using Weth.Actions;
using Weth.Conversation;
//using System.Reflection;

namespace Weth;

internal partial class ModEntry : SimpleMod
{
    private static void Apply(Harmony harmony)
    {
        // Artifacthider
        harmony.Patch(
            original: typeof(ArtifactReward).GetMethod("GetBlockedArtifacts", AccessTools.all),
            postfix: new HarmonyMethod(typeof(Artifacthider), nameof(Artifacthider.ArtifactRewardPreventer))
        );
        harmony.Patch(
            original: typeof(ArtifactReward).GetMethod(nameof(ArtifactReward.GetOffering), AccessTools.all),
            postfix: new HarmonyMethod(typeof(Artifacthider), nameof(Artifacthider.FocusedSpaceRelicsAlwaysRelicRelic))
        );

        // SplitshotTranspiler
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.IgnoreMissingDroneCheck))
        );
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.IgnoreDroneBloops))
        );
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.DontDoDuplicateArtifactModifiers))
        );
        // harmony.Patch(
        //     original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
        //     transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.FuckYouIllDoWhatIWant))
        // );
        // harmony.Patch(
        //     original: typeof(Combat).GetMethod("BeginCardAction", AccessTools.all),
        //     prefix: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.FuckYouIllDoWhatIWantAgain))
        // );
        harmony.Patch(
            original: typeof(AJupiterShoot).GetMethod("Begin", AccessTools.all),
            prefix: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.FlipModDataFromJupiter))
        );
        harmony.Patch(
            original: typeof(Card).GetMethod("MakeAllActionIcons", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.RenderSplitshotAsAttack))
        );
        harmony.Patch(
            original: typeof(Card).GetMethod("RenderAction", AccessTools.all),
            prefix: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(SplitshotTranspiler.IconRenderingStuff))
        );

        // ChoiceRelicRewardOfYourRelicChoice
        harmony.Patch(
            original: typeof(Events).GetMethod("ChoiceCardRewardOfYourColorChoice", AccessTools.all),
            postfix: new HarmonyMethod(typeof(ChoiceRelicRewardOfYourRelicChoice), nameof(ChoiceRelicRewardOfYourRelicChoice.ReplaceCardRewardWithRelic))
        );

        // ArtifactMadcapPartOperator
        harmony.Patch(
            original: typeof(AStunPart).GetMethod("Begin", AccessTools.all),
            prefix: new HarmonyMethod(typeof(ArtifactMadcapPartOperator), nameof(ArtifactMadcapPartOperator.DetectIntent)),
            postfix: new HarmonyMethod(typeof(ArtifactMadcapPartOperator), nameof(ArtifactMadcapPartOperator.DetectChange))
        );

        // ArtifactPowersprintEvadeOperator
        harmony.Patch(
            original: typeof(AStatus).GetMethod("Begin", AccessTools.all),
            prefix: new HarmonyMethod(typeof(ArtifactPowersprintEvadeOperator), nameof(ArtifactPowersprintEvadeOperator.FindEvade))
        );

        // WethArtAndFrameSwitcher
        harmony.Patch(
            original: typeof(Events).GetMethod(nameof(Events.RunWinWho), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(WethArtAndFrameSwitcher.SwitchTheArt))
        );
        harmony.Patch(
            original: typeof(State).GetMethod(nameof(State.GoToZone), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(WethArtAndFrameSwitcher.SwitchTheFrame))
        );
        harmony.Patch(
            original: typeof(Vault).GetMethod(nameof(Vault.GetVaultMemories), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(WethArtAndFrameSwitcher.SwitchTheFrameInVault))
        );
        harmony.Patch(
            original: typeof(State).GetMethod(nameof(State.Update), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(WethArtAndFrameSwitcher.ReapplyFrameOnStartup))
        );
        harmony.Patch(
            original: typeof(Vault).GetMethod(nameof(Vault.LoadFromVault), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethArtAndFrameSwitcher), nameof(WethArtAndFrameSwitcher.UseMemoryFrame))
        );

        // WethForceAdvanceDialogue
        harmony.Patch(
            original: typeof(Dialogue).GetMethod(nameof(Dialogue.OnInputPhase), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethForceAdvanceDialogue), nameof(WethForceAdvanceDialogue.ForceDialogueOnScream))
        );
        harmony.Patch(
            original: typeof(Character).GetMethod(nameof(Character.DrawFace), AccessTools.all),
            postfix: new HarmonyMethod(typeof(WethForceAdvanceDialogue), nameof(WethForceAdvanceDialogue.DrawWethCharOverlay))
        );

        // BattleStimulation helper
        harmony.Patch(
            original: typeof(Ship).GetMethod(nameof(Ship.DirectHullDamage), AccessTools.all),
            postfix: new HarmonyMethod(typeof(BattleStimulationHelper), nameof(BattleStimulationHelper.DetectEnemyLoseHull))
        );

        // Relic Tooltip fixer (when being displayed in the relic offerings)
        harmony.Patch(
            original: typeof(Artifact).GetMethod(nameof(Artifact.GetTooltips), AccessTools.all),
            postfix: new HarmonyMethod(typeof(FixTheRelicTooltips), nameof(FixTheRelicTooltips.FixIt))
        );
    }
}