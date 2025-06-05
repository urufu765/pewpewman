using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

internal class ArtifactDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>(){
            {"AritfactAresCannonV2_Weth_0", new(){
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = ["AresCannonV2"],
                oncePerRunTags = ["AresCannonV2"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "sly", "Now that's what I'm talking about!")
                ]
            }},
            {"ArtifactAresCannon_Weth_0", new(){
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = ["AresCannon"],
                oncePerRunTags = ["AresCannon"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "If only both of these cannons could be used at once... now wouldn't that be neat?")
                ]
            }},
            {"ArtifactArmoredBay_Weth_0", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 1,
                hasArtifacts = ["ArmoredBay"],
                oncePerRunTags = ["ArmoredBae"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "It was about time the missile bay got reinforced, the amount of stray rocks that hit that part from me blowing asteroids up...")
                ]
            }},
            {"ArtifactBerserkerDrive_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 8,
                oncePerRun = true,
                hasArtifacts = ["BerserkerDrive"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "DESTROY THEM ALL!!!")
                ]
            }},
            {"ArtifactBrokenGlasses_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["BrokenGlasses"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "sad", "Aw man, I miss Cleo already.")
                ]
            }},
            {"ArtifactCockpitTargetIsRelevant_Weth_0", new(){
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
            }},
            {"ArtifactCockpitTargetIsNotRelevant_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["CockpitTarget"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                enemyDoesNotHavePart = "cockpit",
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "shrug", "No cockpit? No problem. We just strike them until they blow.")
                ]
            }},
            {"ArtifactCrosslink_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["Crosslink"],
                lookup = ["CrosslinkTrigger"],
                oncePerRunTags = ["CrosslinkTriggering"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "We're so going to the end of the universe and back with this!")
                ]
            }},
            {"ArtifactDirtyEngines_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["DirtyEngines"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "squint", "Hey, is it just me, or does the air feel... chunky?")
                ]
            }},
            {"ArtifactEnergyPrep_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["EnergyPrep"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Ready for action!")
                ]
            }},
            {"ArtifactEnergyRefund_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["EnergyRefund"],
                oncePerCombatTags = ["EnergyRefund"],
                oncePerRun = true,
                minCostOfCardJustPlayed = 3,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Getting in a bonus shot after a big spend is a big plus!")
                ]
            }},
            {"ArtifactFractureDetection_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["FractureDetection"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = ["FractureDetectionBarks"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "bringiton", "Just line me up with the fracture, and I'll make sure to hit the bullseye!")
                ]
            }},
            {"ArtifactGeminiCore_Multi_4", new(){
                edit = [
                    new("af738a7e", AmWeth, "dontcare", "Despite what the giant crystal growing out of my left eye might tell you, I'm more of a red kinda guy.")
                ]
            }},
            {"ArtifactGeminiCoreBooster_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["GeminiCoreBooster"],
                oncePerRunTags = ["GeminiCoreBooster"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "This has solidified my belief in the red side.")
                ]
            }},
            {"ArtifactGeminiCoreBooster_Weth_1", new(){
                type = NodeType.combat,
                hasArtifacts = ["GeminiCoreBooster"],
                oncePerRunTags = ["GeminiCoreBooster"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Blue baaaaaaad, red gooooooood!")
                ]
            }},
            {"ArtifactGravelRecycler_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["GravelRecycler"],
                anyDrones = ["asteroid"],
                oncePerCombatTags = ["GravelRecycler"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "So you're telling me, breaking these lumps of trash gives us protection? Sweet.")
                ]
            }},
            {"ArtifactHardmode_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["HARDMODE"],
                oncePerRunTags = ["HARDMODE"],
                priority = true,
                once = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "squint", "The enemy's moving a bit faster than usual... just me?")
                ]
            }},
            {"ArtifactJetThrustersNoRiggs_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["JetThrusters"],
                maxTurnsThisCombat = 1,
                turnStart = true,
                nonePresent = [AmRiggs],
                oncePerRunTags = ["OncePerRunThrusterJokesAboutRiggsICanMakeTheseTagsStupidlyLongIfIWant"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "shrug", "Even without a proper pilot, lining up shots ain't too bad.")
                ]
            }},
            {"ArtifactOverclockedGenerator_Weth_0", new(){
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
            }},
            {"ArtifactPiercer_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["Piercer"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = ["PiercerShouts"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "hmm", "I mean I'm not opposed to higher firepower but we're definitely turning the piercer off while excavating.")
                ]
            }},
            {"ArtifactPowerDiversionMadeWethAttackFail_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["PowerDiversion"],
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = AmWethDeck,
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmWeth, "wtf", "Hey, what gives?"),
                    new(AmPeri, "Sorry...")
                ]
            }},
            {"ArtifactPowerDiversionMadeWethAttackFail_Weth_1", new(){
                type = NodeType.combat,
                hasArtifacts = ["PowerDiversion"],
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = AmWethDeck,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "hmm", "My shots aren't hitting as hard as I thought it would...")
                ]
            }},
            {"ArtifactPowerDiversionMadeWethAttackFail_Weth_2", new()
            {
                type = NodeType.combat,
                hasArtifacts = ["PowerDiversion"],
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = AmWethDeck,
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmWeth, "squint", "Can this power diversion thing be switched off?"),
                    new(AmPeri, "No.")
                ]
            }},
            {"ArtifactPowerDiversionMadeWethAttackFail_Weth_3", new(){
                type = NodeType.combat,
                hasArtifacts = ["PowerDiversion"],
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = AmWethDeck,
                allPresent = [AmPeri],
                dialogue = [
                    new(AmPeri, "Let me take care of the offense, alright?")
                ]
            }},
            {"ArtifactQuickDraw_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["Quickdraw"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = ["QuickDrawShouts"],
                oncePerRun = true,
                allPresent = [AmWeth, AmRiggs],
                dialogue = [
                    new(AmRiggs, "Options ready!"),
                    new(AmWeth, "sparkle", "Woah!")
                ]
            }},
            {"ArtifactRecalibrator_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                hasArtifacts = ["Recalibrator"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "Usually when I miss, nothing happens!")
                ]
            }},
            {"ArtifactRevengeDrive_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["RevengeDrive"],
                minDamageDealtToPlayerThisTurn = 1,
                enemyShotJustHit = true,
                oncePerCombatTags = ["RevengeDriveShouts"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "bringiton", "Let's show them how it's REALLY done!")
                ]
            }},
            {"ArtifactTiderunner_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["TideRunner"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = ["TideRunner"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Personally, I don't like the wood finish. I like my ship parts all metal if possible.")
                ]
            }},
            {"ArtifactTridimensionalCockpit_Weth_0", new(){
                type = NodeType.combat,
                hasArtifacts = ["TridimensionalCockpit"],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = ["TridimensionalCockpit"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "squint", "This is... disorienting.")
                ]
            }},


            {"ArtifactTreasureHunterWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureHunter)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureHunterWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "With enough bits and pieces floating around, I bet I could find something useful!")
                ]
            }},
            {"ArtifactTreasureHunterWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureHunter)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureHunterWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "I'll be keeping my eyes peeled for goodies!")
                ]
            }},
            {"ArtifactTreasureHunterWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureHunter)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureHunterWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "Batter them enough and I bet we'll salvage something useful.")
                ]
            }},
            {"ArtifactTreasureSeekerWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureSeeker)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureSeekerWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "shrug", "If finding goodies isn't a good enough incentive to keep shooting, I don't know what is!")
                ]
            }},
            {"ArtifactTreasureSeekerWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureSeeker)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureSeekerWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "The more bits that fly off the enemy, the more chances I get to grabbing something useful!")
                ]
            }},
            {"ArtifactTreasureSeekerWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TreasureSeeker)],
                playerShotJustHit = true,
                oncePerRunTags = ["TreasureSeekerWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "dontcare", "Even scratched off paint could be useful for something.")
                ]
            }},
            {"ArtifactExcursionWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusMilestone)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethGoesOnAnExcursion"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "It's time to go on an excursion!")
                ]
            }},
            {"ArtifactExcursionWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusMilestone)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethGoesOnAnExcursion"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "With experience comes more opportunities.")
                ]
            }},
            {"ArtifactExcursionWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusMilestone)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethGoesOnAnExcursion"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "sly", "There's always something new to discover from blowing up people you don't like.")
                ]
            }},
            {"ArtifactSpaceRelicsWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SpaceRelics2)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "sparkle", "The shiny stuff is very shiny...")
                ]
            }},
            {"ArtifactSpaceRelicsWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SpaceRelics2)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "The ship already feels like it's running better.")
                ]
            }},
            {"ArtifactSpaceRelicsWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SpaceRelics2)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmBooks, AmWeth],
                dialogue = [
                    new(AmBooks, "That's a shiny rock!"),
                    new(AmWeth, "sparkle", "Very.")
                ]
            }},
            {"ArtifactCracklingRelicsWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Crackling)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "sparkle", "Wow! I can feel the energy radiating from the rock!")
                ]
            }},
            {"ArtifactCracklingRelicsWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Crackling)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmDizzy, AmWeth],
                dialogue = [
                    new(AmDizzy, "squint", "That's not radioactive, right?"),
                    new([
                        new(AmWeth, "shrug", "I have absolutely no idea."),
                        new(AmWeth, "dontcare", "You only live once."),
                        new(AmWeth, "intense", "I... I forgot to test. Hopefully not.")
                    ])
                ]
            }},
            {"ArtifactSubsumingRelicsWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Subsuming)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "I sorta expected this to give off energy, but instead it feels like it's absorbing it?")
                ]
            }},
            {"ArtifactSubsumingRelicsWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Subsuming)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "whatsthis", "Why do I feel so drained?")
                ]
            }},
            {"ArtifactFocusedRelicsWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Focused)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "crystal", "...")
                ]
            }},
            {"ArtifactFocusedRelicsWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SR2Focused)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["WethsSpaceRelic"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "intense", "Why do I have the sudden urge to collect the same things over and over again?")
                ]
            }},
            { "ArtifactJauntWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusJaunt)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["JauntAllTheWay"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Let's go on an adventure!")
                ]
            }},
            {"ArtifactJauntWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusJaunt)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["JauntAllTheWay"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "hmm", "I can smell the hidden treasure, just gotta light them up to find it!")
                ]
            }},
            {"ArtifactJauntWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(TerminusJaunt)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["JauntAllTheWay"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "squint", "It's hidden somewhere. The thing. I can feel it.")
                ]
            }},
        });

        LocalDB.DumpStoryToLocalLocale("en", "Shockah.DuoArtifacts", new Dictionary<string, DialogueMachine>(){
            {"ArtifactRockPowerWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(RockPower)],
                anyDrones = ["asteroid"],
                oncePerCombatTags = ["rockPowerAdvice"],
                allPresent = [AmWeth, AmIsaac],
                dialogue = [
                    new(AmWeth, "squint", "Hey, I think I see something in the lumps of rock."),
                    new(AmIsaac, "Well, what are you waiting for?")
                ]
            }},
            {"ArtifactRockPowerWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(RockPower)],
                anyDrones = ["asteroid"],
                oncePerCombatTags = ["rockPowerAdvice"],
                allPresent = [AmWeth, AmIsaac],
                dialogue = [
                    new(AmWeth, "pointing", "There might be something usable in the rocks!"),
                    new(AmIsaac, "explains", "Took you long enough to figure that out.")
                ]
            }},
            {"ArtifactResidualShotNoDamageWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(ResidualShot)],
                whoDidThat = AmWethDeck,
                maxDamageDealtToEnemyThisAction = 0,
                oncePerRunTags = ["wethsResidualShotUseless"],
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmWeth, "shrug", "It's the thought that counts."),
                    new(AmPeri, "mad", "Try again.")
                ]
            }},
            {"ArtifactResidualShotNoDamageWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(ResidualShot)],
                whoDidThat = AmWethDeck,
                maxDamageDealtToEnemyThisAction = 0,
                oncePerRunTags = ["wethsResidualShotUseless"],
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmWeth, "dontcare", "Better than not having it."),
                    new(AmPeri, "squint", "Not very helpful either.")
                ]
            }},
            {"ArtifactResidualShotNoDamageWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(ResidualShot)],
                whoDidThat = Deck.peri,
                maxDamageDealtToEnemyThisAction = 0,
                oncePerRunTags = ["perisResidualShotUseless"],
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmPeri, "mad", "What was that?!"),
                    new(AmWeth, "explain", "You missing.")
                ]
            }},
            {"ArtifactResidualShotNoDamageWeth_Multi_3", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(ResidualShot)],
                whoDidThat = Deck.peri,
                maxDamageDealtToEnemyThisAction = 0,
                oncePerRunTags = ["perisResidualShotUseless"],
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmPeri, "mad", "Who tampered with the cannon?"),
                    new(AmWeth, "pointout", "Don't blame the equipment.")
                ]
            }},
            {"ArtifactCannonRechargeWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CannonRecharge)],
                lookup = ["CannonRechargeTrigger"],
                oncePerRun = true,
                priority = true,
                oncePerCombatTags = ["CannonRechargeTag"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Energy successfully routed to stun shot!")
                ]
            }},
            {"ArtifactCannonRechargeWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CannonRecharge)],
                lookup = ["CannonRechargeTrigger"],
                oncePerRun = true,
                priority = true,
                oncePerCombatTags = ["CannonRechargeTag"],
                allPresent = [AmWeth, AmCat],
                dialogue = [
                    new(AmWeth, "sly", "Next shot should be shocking!"),
                    new(AmCat, "squint", "Get out.")
                ]
            }},
            {"ArtifactMadcapChargeWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(MadcapCharge)],
                whoDidThat = Deck.dizzy,
                oncePerRunTags = ["Madcapper"],
                lookup = ["madcapped"],
                allPresent = [AmWeth, AmDizzy],
                dialogue = [
                    new(AmDizzy, "Your turn."),
                    new(AmWeth, "Tag team!")
                ]
            }},
            {"ArtifactMadcapChargeWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(MadcapCharge)],
                whoDidThat = Deck.dizzy,
                oncePerRunTags = ["Madcapper"],
                lookup = ["madcapped"],
                allPresent = [AmWeth, AmDizzy],
                dialogue = [
                    new(AmDizzy, "Let 'er rip!"),
                    new(AmWeth, "lockedin", "Yessir!")
                ]
            }},
            {"ArtifactSprintSprintBabyWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(PowerSprint)],
                oncePerRunTags = ["SprintToTheEnd"],
                lookup = ["sprintmobiled"],
                allPresent = [AmWeth, AmRiggs],
                dialogue = [
                    new(AmRiggs, "squint", "Hey, where did the power go?"),
                    new(AmWeth, "lockedin", "Guns ready!")
                ]
            }},
            {"ArtifactSprintSprintBabyWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(PowerSprint)],
                oncePerRunTags = ["SprintToTheEnd"],
                lookup = ["sprintmobiled"],
                allPresent = [AmWeth, AmRiggs],
                dialogue = [
                    new(AmRiggs, "squint", "Hey, I was gonna use that."),
                    new(AmWeth, "sly", "Mine now.")
                ]
            }},
            {"ArtifactPyeorocanonWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(PyroCannon)],
                oncePerRunTags = ["PyroGoBoom"],
                lastTurnPlayerStatuses = [Status.heat],
                allPresent = [AmWeth, AmDrake],
                dialogue = [
                    new(AmWeth, "squint", "Did I connect this right?"),
                    new(AmDrake, "I dunno, but heat equals more power now!")
                ]
            }},
            {"ArtifactPyeorocanonWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(PyroCannon)],
                oncePerRunTags = ["PyroGoBoom"],
                lastTurnPlayerStatuses = [Status.heat],
                allPresent = [AmWeth, AmDrake],
                dialogue = [
                    new(AmDrake, "sly", "I just do what I usually do and you get more power, right?"),
                    new(AmWeth, "hmm", "I fear I've made a critical mistake."),
                ]
            }},
            // {"ArtifactHiddenGamWeth_Multi_0", new(){
            //     type = NodeType.combat,
            //     hasArtifactTypes = [typeof(HiddenGem)],
            //     oncePerRunTags = ["oohshiny"],

            //     allPresent = [AmWeth, AmMax],
            //     dialogue = [
            //         new(AmMax, "Hey Weth, look what I found!"),
            //         new(AmWeth, "Don't bring that near me.")
            //     ]
            // }},
            {"ArtifactMoPowahBabyWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(PowerCrystals)],
                oncePerRunTags = ["crystalweth"],
                lastTurnPlayerStatuses = [ Status.shard ],
                allPresent = [AmWeth, AmBooks],
                dialogue = [
                    new(AmBooks, "Wow! These shards are vibrating!"),
                    new(AmWeth, "Quick! Load them into the cannon!")
                ]
            }},
            {"ArtifactBattleStimulationWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(BattleStimulation)],
                oncePerRunTags = ["wethBattleStimulationActivated"],
                lookup = ["wethStimuliActive"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "I feel like I can take on the whole world.")
                ]
            }},
            {"ArtifactBattleStimulationWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(BattleStimulation)],
                oncePerRunTags = ["wethBattleStimulationActivated"],
                lookup = ["wethStimuliActive"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Woh wow, that's a feeling I haven't felt in a while!")
                ]
            }},
            {"ArtifactBattleStimulationWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(BattleStimulation)],
                oncePerRunTags = ["wethBattleStimulationActivated"],
                lookup = ["wethStimuliActive"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedon", "Aww yeah! Let's blow stuff up!")
                ]
            }},
            { "ArtifactCalculatedWhiffWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CalculatedWhiff)],
                oncePerRunTags = ["wethFuckingMissedOnPurpose"],
                playerShotJustMissed = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "dontcare", "I'll make sure the next shots hurt a lot more.")
                ]
            }},
            { "ArtifactCalculatedWhiffWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CalculatedWhiff)],
                oncePerRunTags = ["wethFuckingMissedOnPurpose"],
                playerShotJustMissed = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "shrug", "Can't have power without sacrifice.")
                ]
            }},
            { "ArtifactCalculatedWhiffWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CalculatedWhiff)],
                oncePerRunTags = ["wethFuckingMissedOnPurpose"],
                playerShotJustMissed = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "I know what I'm doing.")
                ]
            }},
            { "ArtifactCannonPrimerWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(CannonPrimer)],
                oncePerRun = true,
                lookup = ["cannonPrimedWeth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "One should always warm up the cannons after not using them for a while.")
                ]
            }},
            {"ArtifactAllPowerToCannonsWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(AlPoToCa)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["APTCweth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lookatyou", "Alright, just let me know when to go full berserk mode.")
                ]
            }},
            {"ArtifactAllPowerToCannonsWeth_Multi_1", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(AlPoToCa)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["APTCweth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lookatyou", "Whenever y'all're ready.")
                ]
            }},
            {"ArtifactAllPowerToCannonsWeth_Multi_2", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(AlPoToCa)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["APTCweth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lookatyou", "Just don't hold off for too long, 'kay?")
                ]
            }},
            {"ArtifactAllPowerToCannonsWeth_Multi_3", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(AlPoToCa)],
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = ["APTCweth"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lookatyou", "We've got one chance at this, make it count.")
                ]
            }},
            {"ArtifactSuperDriveCollectorWeth_Multi_0", new(){
                type = NodeType.combat,
                hasArtifactTypes = [typeof(SuperDriveCollector)],
                oncePerRun = true,
                lastTurnPlayerStatuses = [ Status.powerdrive, Status.overdrive, IsPulsedrive, IsMinidrive],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "yay", "We got ALL the drives!")
                ]
            }},
        });

        LocalDB.DumpStoryToLocalLocale("en", "urufudoggo.Illeana", new Dictionary<string, DialogueMachine>()
        {
            {"ArtifactForgedCertificate_Illeana_0", new(){
                dialogue = [
                    new(),
                    new(AmWeth, "squint", "Hey, no?")
                ]
            }}
        });

    }
}
