using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

internal class ArtifactOldDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.LocalStory.all["AritfactAresCannonV2_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            turnStart = true,
            maxTurnsThisCombat = 1,
            hasArtifacts = ["AresCannonV2"],
            oncePerRunTags = ["AresCannonV2"],
            allPresent = [AmWeth],
            dialogue = [
            new(AmWeth, "sparkle", "Now that's what I'm talking about!")
            ]
        };
        LocalDB.LocalStory.all["AritfactAresCannon_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            turnStart = true,
            maxTurnsThisCombat = 1,
            hasArtifacts = ["AresCannon"],
            oncePerRunTags = ["AresCannon"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "If only both of these cannons could be used at once... now wouldn't that be neat?")
            ]
        };
        LocalDB.LocalStory.all["ArtifactArmoredBay_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            enemyShotJustHit = true,
            minDamageBlockedByPlayerArmorThisTurn = 1,
            hasArtifacts = ["ArmoredBay"],
            oncePerRunTags = ["ArmoredBae"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "explain", "It was about time the missile bay got reinforced, the amount of stray rocks that hit that part from me blowing asteroids up...")
            ]
        };
        LocalDB.LocalStory.all["ArtifactBerserkerDrive_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            playerShotJustHit = true,
            minDamageDealtToEnemyThisTurn = 8,
            oncePerRun = true,
            hasArtifacts = ["BerserkerDrive"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "lockedin", "DESTROY THEM ALL!!!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactBrokenGlasses_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["BrokenGlasses"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "sad", "Aw man, I miss Cleo already.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactCockpitTargetIsRelevant_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["CockpitTarget"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerRun = true,
            enemyHasPart = "cockpit",
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "lockedin", "Target their seeing port! Make them BLIND!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactCockpitTargetIsNotRelevant_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["CockpitTarget"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerRun = true,
            enemyDoesNotHavePart = "cockpit",
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "explain", "No cockpit? No problem. We just strike them until they blow.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactCrosslink_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["Crosslink"],
            lookup = ["CrosslinkTrigger"],
            oncePerRunTags = ["CrosslinkTriggering"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "We're so going to the end of the universe and back with this!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactDirtyEngines_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["DirtyEngines"],
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "squint", "Hey, is it just me, or does the air feel... chunky?")
            ]
        };
        LocalDB.LocalStory.all["ArtifactEnergyPrep_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["EnergyPrep"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Ready for action!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactEnergyRefund_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["EnergyRefund"],
            oncePerCombatTags = ["EnergyRefund"],
            oncePerRun = true,
            minCostOfCardJustPlayed = 3,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Getting in a bonus shot after a big spend is a big plus!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactFractureDetection_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["FractureDetection"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerCombatTags = ["FractureDetectionBarks"],
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Just line me up with the fracture, and I'll make sure to hit the bullseye!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactGeminiCore_Multi_4"] = new DialogueMachine
        {
            edit = [
                new("af738a7e", AmWeth, "explain", "Despite what the giant crystal growing out of my left eye might tell you, I'm more of a red kinda guy.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactGeminiCoreBooster_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["GeminiCoreBooster"],
            oncePerRunTags = ["GeminiCoreBooster"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "This has solidified my belief in the red side.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactGeminiCoreBooster_Weth_1"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["GeminiCoreBooster"],
            oncePerRunTags = ["GeminiCoreBooster"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "sparkle", "Blue baaaaaaad, red gooooooood!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactGravelRecycler_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["GravelRecycler"],
            anyDrones = ["asteroid"],
            oncePerCombatTags = ["GravelRecycler"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "So you're telling me, breaking these lumps of trash gives us protection? Sweet.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactHardmode_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["HARDMODE"],
            oncePerRunTags = ["HARDMODE"],
            priority = true,
            once = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "squint", "The enemy's moving a bit faster than usual... just me?")
            ]
        };
        LocalDB.LocalStory.all["ArtifactJetThrustersNoRiggs_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["JetThrusters"],
            maxTurnsThisCombat = 1,
            turnStart = true,
            nonePresent = [AmRiggs],
            oncePerRunTags = ["OncePerRunThrusterJokesAboutRiggsICanMakeTheseTagsStupidlyLongIfIWant"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Even without a proper pilot, lining up shots ain't too bad.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactOverclockedGenerator_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["OverclockedGenerator"],
            oncePerRunTags = ["OverclockedGeneratorTag"],
            lookup = ["OverclockedGeneratorTrigger"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Reminds me of the time when I had to overclock my ship's generator to even get it to move."),
                new([
                    new(AmMax, "That's concerning?"),
                    new(AmBooks, "Is that how you got a crystal growing out of your face?"),
                    new(AmDizzy, "squint", "How bad was your old ship?")
                ])
            ]
        };
        LocalDB.LocalStory.all["ArtifactPiercer_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["Piercer"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerCombatTags = ["PiercerShouts"],
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "I mean I'm not opposed to higher firepower but we're definitely turning the piercer off while excavating.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactPowerDiversionMadeWethAttackFail_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["PowerDiversion"],
            playerShotJustHit = true,
            maxDamageDealtToEnemyThisAction = 0,
            whoDidThat = AmWethDeck,
            allPresent = [AmWeth, AmPeri],
            dialogue = [
                new(AmWeth, "Hey, what gives?"),
                new(AmPeri, "Sorry...")
            ]
        };
        LocalDB.LocalStory.all["ArtifactPowerDiversionMadeWethAttackFail_Weth_1"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["PowerDiversion"],
            playerShotJustHit = true,
            maxDamageDealtToEnemyThisAction = 0,
            whoDidThat = AmWethDeck,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "squint", "My shots aren't hitting as hard as I thought it would...")
            ]
        };
        LocalDB.LocalStory.all["ArtifactPowerDiversionMadeWethAttackFail_Weth_2"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["PowerDiversion"],
            playerShotJustHit = true,
            maxDamageDealtToEnemyThisAction = 0,
            whoDidThat = AmWethDeck,
            allPresent = [AmWeth, AmPeri],
            dialogue = [
                new(AmWeth, "Can this power diversion thing be switched off?"),
                new(AmPeri, "No.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactPowerDiversionMadeWethAttackFail_Weth_3"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["PowerDiversion"],
            playerShotJustHit = true,
            maxDamageDealtToEnemyThisAction = 0,
            whoDidThat = AmWethDeck,
            allPresent = [AmPeri],
            dialogue = [
                new(AmPeri, "Let me take care of the offense, alright?")
            ]
        };
        LocalDB.LocalStory.all["ArtifactQuickDraw_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["QuickDraw"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerCombatTags = ["QuickDrawShouts"],
            oncePerRun = true,
            allPresent = [AmWeth, AmRiggs],
            dialogue = [
                new(AmRiggs, "Options ready!"),
                new(AmWeth, "sparkle", "Woah!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactRecalibrator_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            playerShotJustMissed = true,
            hasArtifacts = ["Recalibrator"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "sparkle", "Usually when I miss, nothing happens!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactRevengeDrive_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["RevengeDrive"],
            minDamageDealtToPlayerThisTurn = 1,
            enemyShotJustHit = true,
            oncePerCombatTags = ["RevengeDriveShouts"],
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "Let's show them how it's REALLY done!")
            ]
        };
        LocalDB.LocalStory.all["ArtifactTiderunner_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["TideRunner"],
            turnStart = true,
            maxTurnsThisCombat = 1,
            oncePerCombatTags = ["TideRunner"],
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "explain", "Personally, I don't like the wood finish. I like my ship parts all metal if possible.")
            ]
        };
        LocalDB.LocalStory.all["ArtifactTridimensionalCockpit_Weth_0"] = new DialogueMachine
        {
            type = NodeType.combat,
            hasArtifacts = ["TridimensionalCockpit"],
            turnStart = true,
            maxTurnsThisCombat = 1, 
            oncePerCombatTags = ["TridimensionalCockpit"],
            oncePerRun = true,
            allPresent = [AmWeth],
            dialogue = [
                new(AmWeth, "This is... disorienting.")
            ]
        };
    }
}
