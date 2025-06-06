using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

internal class CombatDialogue : IRegisterable, IDialogueRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>(){
            {"BanditThreats_Multi_0", new(){
                edit = [new("1bcc0802", AmWeth, "intense", "Hey? No?")]
            }},
            {"BatboyKeepsTalking_Multi_0", new(){
                edit = [new("5b1666a6", AmWeth, "plead", "Will you stop trying to kill us if we say yes?")]
            }},
            {"BlockedALotOfAttacksWithArmor_Weth_0", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 3,
                oncePerCombatTags = ["YowzaThatWasALOTofArmorBlock"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "Okay, reminder to self, don't do what the enemy is doing...")
                ]
            }},
            {"BooksWentMissing_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lastTurnPlayerStatuses = [Status.missingBooks],
                priority = true,
                oncePerCombatTags = ["booksWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "plead", "Where did she go?!")
                ]
            }},
            {"CheapCardPlayed_Weth_0", new(){
                type = NodeType.combat,
                maxCostOfCardJustPlayed = 0,
                oncePerCombatTags = ["CheapCardPlayed"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "Every bit helps.")
                ]
            }},
            {"CrabFacts1_Multi_0", new(){
                edit = [
                    new("4271d60b", AmWeth, "Interesting, I did not know that.")
                ]
            }},
            {"CrabFacts2_Multi_0", new(){
                edit = [
                    new("d4309f0d", AmWeth, "squint", "You're losing me.")
                ]
            }},
            {"CrabFactsAreOverNow_Multi_0", new(){
                edit = [
                    new("82c0273b", AmWeth, "mad", "That's it?!")
                ]
            }},
            {"DillianShouts", new(){
                edit = [new("b01729fe", AmWeth, "pointout", "Who said you could be my sworn enemy?")]
            }},
            {"DizzyWentMissing_Weth_0", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [Status.missingDizzy],
                priority = true,
                oncePerCombatTags = ["dizzyWentMissing"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "panic", "OH NO! Our defense guy!")
                ]
            }},
            {"DualNotEnoughDronesShouts_Multi_0", new(){
                edit = [new("650e6806", AmWeth, "I can take care of that for you!")]
            }},
            {"DualNotEnoughDronesShouts_Multi_2", new(){
                edit = [new("9b0ce906", AmWeth, "pointout", "You're doing a terrible job.")]
            }},
            {"Duo_AboutToDieAndLoop_Weth_0", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmDizzy],
                dialogue = [
                    new(AmWeth, "sad", "There's so much I couldn't see..."),
                    new(AmDizzy, "frown", "Maybe next time.")
                ]
            }},
            {"Duo_AboutToDieAndLoop_Weth_1", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmIsaac],
                dialogue = [
                    new(AmWeth, "verysad", "How cruel."),
                    new(AmIsaac, "Let's do better next time.")
                ]
            }},
            {"Duo_AboutToDieAndLoop_Weth_2", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmRiggs],
                dialogue = [
                    new(AmWeth, "sad", "I guess this is it..."),
                    new(AmRiggs, "Hey, we're not dead yet!")
                ]
            }},
            {"Duo_AboutToDieAndLoop_Weth_3", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmMax],
                dialogue = [
                    new(AmWeth, "sob", "So many regrets!"),
                    new(AmMax, "intense", "Oh no.")
                ]
            }},
            {"Duo_AboutToDieAndLoop_Weth_4", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmPeri],
                dialogue = [
                    new(AmWeth, "sad", "At least I experienced this with you guys..."),
                    new(AmPeri, "mad", "That's sweet. Now get back to your station.")
                ]
            }},
            {"Duo_AboutToDieAndLoop_Weth_5", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                allPresent = [AmWeth, AmBooks],
                dialogue = [
                    new(AmBooks, "See you again, Miss Weth!"),
                    new(AmWeth, "sad", "Sure...")
                ]
            }},
            {"EnemyArmorHitLots_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                playerShotJustHit = true,
                minDamageBlockedByEnemyArmorThisTurn = 3,
                oncePerCombat = true,
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "hmm", "I was kinda expecting to drill right through the armor...")
                ]
            }},
            {"EnemyArmorPierced_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                playerJustPiercedEnemyArmor = true,
                oncePerCombatTags = ["EnemyArmorPierced"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "think", "If this could do that to armor, imagine what it could do to a stubborn rock!")
                ]
            }},
            {"EnemyHasBrittle_Weth_0", new(){
                type = NodeType.combat,
                enemyHasBrittlePart = true,
                oncePerRunTags = ["yelledAboutBrittle"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "Guess what time it is? It's time for you to meet your maker!")
                ]
            }},
            {"EnemyHasWeakness_Weth_0", new(){
                type = NodeType.combat,
                enemyHasWeakPart = true,
                oncePerRunTags = ["yelledAboutWeakness"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "Oh this is gonna hurt... for them!")
                ]
            }},
            {"ExpensiveCardPlayed_Weth_0", new(){
                type = NodeType.combat,
                minCostOfCardJustPlayed = 4,
                oncePerCombatTags = ["ExpensiveCardPlayed"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "We got a lot out of that power drain... probably.")
                ]
            }},
            {"ExpensiveCardPlayed_Weth_1", new(){
                type = NodeType.combat,
                minCostOfCardJustPlayed = 4,
                whoDidThat = AmWethDeck,
                oncePerCombatTags = ["ExpensiveCardPlayed"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "We hit soft now to hit real hard later.")
                ]
            }},
            {"FreezeIsMaxSize_Weth_0", new(){
                type = NodeType.combat,
                turnStart = true,
                enemyIntent = "biggestCrystal",
                oncePerCombatTags = ["biggestCrystalShout"],
                allPresent = [AmWeth, AmCrystalMiniboss],
                dialogue = [
                    new(AmWeth, "panic", "The amount of dread I'm currently feeling is actually crushing me.")
                ]
            }},
            {"HandOnlyHasTrashCards_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                handFullOfTrash = true,
                oncePerCombatTags = ["handOnlyHasTrashCards"],
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "plead", "This isn't my fault, I swear...")
                ]
            }},
            {"HandOnlyHasUnplayableCards_Weth_0", new(){
                type = NodeType.combat,
                handFullOfUnplayableCards = true,
                oncePerCombatTags = ["handFullOfUnplayableCards"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "squint", "How did this happen?")
                ]
            }},
            {"HandOnlyHasUnplayableCards_Weth_1", new(){
                type = NodeType.combat,
                handFullOfUnplayableCards = true,
                oncePerCombatTags = ["handFullOfUnplayableCards"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "complain", "How did this even happen?")
                ]
            }},
            {"IssacHasTooMuchRockFactoryButYouHaveRockPower_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth, AmIsaac],
                priority = true,
                lookup = ["RockFactoryTooMany"],
                oncePerCombatTags = ["RockFactoryTooManyPower"],
                oncePerRun = true,
                hasArtifactTypes = [typeof(RockPower)],
                dialogue = [
                    new(AmWeth, "think", "This shouldn't be allowed."),
                    new(AmIsaac, "explains", "Oh but it is.")
                ]
            }},
            {"IsaacWentMissing_Weth_0", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [Status.missingIsaac],
                priority = true,
                oncePerCombatTags = ["isaacWentMissing"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "panic", "NOT THE DRONE GUY!")
                ]
            }},
            {"JustHitGeneric_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Hit confirmed!")
                ]
            }},
            {"JustHitGeneric_Weth_1", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "There's more where that came from.")
                ]
            }},
            {"JustHitGeneric_Weth_2", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Take that!")
                ]
            }},
            {"JustHitGeneric_Weth_3", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "Die! Die! Die!")
                ]
            }},
            {"JustHitGeneric_Weth_4", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Target hit.")
                ]
            }},
            {"JustHitGeneric_Weth_5", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Keep it going!")
                ]
            }},
            {"JustHitGeneric_Weth_6", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "Gotcha!")
                ]
            }},
            {"JustHitGeneric_Weth_7", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "explain", "Damage confirmed.")
                ]
            }},
            {"JustPlayedASashaCard_Multi_2", new(){
                edit = [new("7c8cba71", AmWeth, "sparkle", "Oh I love sports ball!")]
            }},
            {"JustPlayedASashaCard_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                whoDidThat = Deck.sasha,
                oncePerRunTags = ["usedASashaCard"],
                dialogue = [
                    new(AmWeth, "plead", "You guys won't mind if I get distracted by sports ball, right?"),
                    new([
                        new(AmDizzy, "shrug", "Go ahead!"),
                        new(AmBooks, "Sports!!"),
                        new(AmMax, "squint", "I'll mind a little."),
                        new(AmPeri, "mad", "Stay focused!")
                    ])
                ]
            }},
            {"JustPlayedASogginsCard_Weth_0", new(){
                type = NodeType.combat,
                whoDidThat = Deck.soggins,
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "donewithit", "I have the countermeasure if anything goes awry.")
                ]
            }},
            {"JustPlayedAToothCard_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                whoDidThat = Deck.tooth,
                minDamageDealtToEnemyThisAction = 1,
                playerShotJustHit = true,
                dialogue = [
                    new(AmWeth, "Even junk can be polished into something useful.")
                ]
            }},
            {"JustPlayedAToothCard_Weth_1", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                whoDidThat = Deck.tooth,
                maxDamageDealtToEnemyThisAction = 0,
                playerShotJustHit = true,
                dialogue = [
                    new(AmWeth, "squint", "If I had more time, I could've actually turned this into something.")
                ]
            }},
            {"JustPlayedAToothCard_Weth_2", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                whoDidThat = Deck.tooth,
                minDamageDealtToEnemyThisAction = 5,
                playerShotJustHit = true,
                dialogue = [
                    new(AmWeth, "explain", "And that is why we don't leave any stone unturned.")
                ]
            }},
            {"LookOutMissile_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth, AmDizzy],
                priority = true,
                once = true,
                oncePerRunTags = ["goodMissileAdvice"],
                anyDronesHostile = [
                    "missile_normal",
                    "missile_heavy",
                    "missile_corrode",
                    "missile_breacher"
                ],
                dialogue = [
                    new(AmDizzy, "Get out of the missile's way!"),
                    new(AmWeth, "No, line me up to shoot it down!")
                ]
            }},
            {"MaxWentMissing_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lastTurnPlayerStatuses = [Status.missingMax],
                priority = true,
                oncePerCombatTags = ["maxWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "plead", "Where's Max?")
                ]
            }},
            {"OneHitPointThisIsFine_Weth_0", new(){
                type = NodeType.combat,
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                enemyShotJustHit = true,
                maxHull = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "surprise", "That was WAY TOO CLOSE for comfort!")
                ]
            }},
            {"OverheatDrakeFix_Multi_6", new(){
                edit = [new(EMod.countFromStart, 1, AmWeth, "squint", "You've made me realize how much I prefer the cold.")]
            }},
            {"OverheatDrakesFault_Multi_9", new(){
                edit = [new(EMod.countFromStart, 1, AmWeth, "pain", "This heat is unbearable.")]
            }},
            {"PeriWentMissing_Weth_0", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [Status.missingPeri],
                priority = true,
                oncePerCombatTags = ["periWentMissing"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "panic", "Ack! Peri!")
                ]
            }},
            {"RiggsWentMissing_Weth_0", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [Status.missingRiggs],
                priority = true,
                oncePerCombatTags = ["riggsWentMissing"],
                oncePerRun = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "intense", "Who's manning the flight controls now?!")
                ]
            }},
            {"SkunkFirstTurnShouts_Multi_0", new(){
                edit = [new("f47e8bf8", AmWeth, "lockedin", "I'm actually here to harvest that drill from you.")]
            }},
            {"ShopKeepBattleInsult", new(){
                edit = [new("66ea84d6", AmWeth, "cryingcat", "NOOOOOOOOOOOOoooooooooooooooo...")]
            }},
            {"SogginsEscapeIntent_1", new(){
                edit = [new("cb0e74d6", AmWeth, "shrug", "Mission... complete?")]
            }},
            {"SogginsEscapeIntent_3", new(){
                edit = [new("d3fa946e", AmWeth, "sly", "Oh, right after I'm done living out my power fantasy.")]
            }},
            {"Soggins_Missile_Shout_1", new(){
                edit = [new("786d2caf", AmWeth, "dontcare", "Turns out I'm good for destroying drones AND ships at the same time.")]
            }},
            {"SpikeGetsChatty_Multi_0", new(){
                edit = [new(EMod.countFromStart, 1, AmWeth, "bringiton", "Bring it on!")]
            }},
            {"StardogGetsChatty_Weth_0", new(){
                type = NodeType.combat,
                turnStart = true,
                allPresent = [AmWeth, AmStardog],
                oncePerCombatTags = ["StardogLeaveUsAlone"],
                dialogue = [
                    new(AmWeth, "bringiton", "Ah! My eternal rival! We meet once more!"),
                    new(AmStardog, "panic", "Who ARE you?!")
                ]
            }},
            {"ThatsALotOfDamageToThem_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 10,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "Now THIS is damage!")
                ]
            }},
            {"ThatsALotOfDamageToThem_Weth_1", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 10,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "YEEEAH! DIE!")
                ]
            }},
            {"ThatsALotOfDamageToThem_Weth_2", new(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 10,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "lockedin", "Break APART! Become salvage!")
                ]
            }},
            {"TookDamageHave2HP_Multi_2", new(){
                edit = [new(EMod.countFromStart, 1, AmWeth, "facepalm", "DON'T- Don't remind me.")]
            }},
            {"VeryManyTurns_Weth_0", new(){
                type = NodeType.combat,
                minTurnsThisCombat = 20,
                oncePerCombatTags = ["veryManyTurns"],
                oncePerRun = true,
                turnStart = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "donewithit", "Are you doing this on purpose?")
                ]
            }},
            {"VeryManyTurns_Weth_1", new(){
                type = NodeType.combat,
                minTurnsThisCombat = 20,
                oncePerCombatTags = ["veryManyTurns"],
                oncePerRun = true,
                turnStart = true,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "apple", "Anyone got a knife?")
                ]
            }},
            {"WeAreCorroded_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lastTurnPlayerStatuses = [Status.corrode],
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "surprise", "Save the rocks! SAVE THE ROCKS!")
                ]
            }},
            {"WeAreMovingAroundALot_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                minMovesThisTurn = 3,
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "facepalm", "I'm starting to see double...")
                ]
            }},
            {"WeDidOverFiveDamage_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 6,
                dialogue = [
                    new(AmWeth, "bringiton", "Meet your maker!")
                ]
            }},
            {"WeDidOverThreeDamage_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 4,
                dialogue = [
                    new(AmWeth, "lockedin", "More! MORE!")
                ]
            }},
            {"WeDontOverlapWithEnemyButSeeker_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = ["NoOverlapBetweenShipsSeeker"],
                anyDronesHostile = ["missile_seeker"],
                nonePresent = ["crab"],
                dialogue = [
                    new(AmWeth, "angrypointing", "See? This is why we should've taken care of the missiles BEFORE doing a tactical retreat!")
                ]
            }},
            {"WeDontOverlapWithEnemyAtAll_Weth_0", new(){
                type = NodeType.combat,
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = ["NoOverlapBetweenShips"],
                oncePerRun = true,
                nonePresent = ["crab", "scrap"],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "angry", "Now how are we supposed to hit them from here?")
                ]
            }},
            {"WeGotHurtButNotTooBad_Weth_0", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "mad", "Argh! How dare they!")
                ]
            }},
            {"WeJustGainedHeatAndDrakeIsHere_Multi_0", new(){
                edit = [new(EMod.countFromStart, 1, AmWeth, "angrypointing", "Drake! Do something about this heat! I'm dying over here!")]
            }},
            {"WeMissedOopsie_Weth_0", new(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifactTypes = [typeof(Recalibrator), typeof(GrazerBeam)],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "shrug", "Eh, not the end of the world.")
                ]
            }},
            {"WeMissedOopsie_Weth_1", new(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifactTypes = [typeof(Recalibrator), typeof(GrazerBeam)],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "There's still a chance.")
                ]
            }},
            {"WeMissedOopsie_Weth_2", new(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifactTypes = [typeof(Recalibrator), typeof(GrazerBeam)],
                allPresent = [AmWeth],
                dialogue = [
                    new(AmWeth, "hmm", "That should scare them... maybe.")
                ]
            }},
            {"WizardGeneralShouts_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth, AmWizbo],
                turnStart = true,
                enemyIntent = "wizardMagic",
                excludedScenes = ["Wizard_Weth_3"],
                dialogue = [
                    new(AmWizbo, "Habadiba!"),
                    new(AmWeth, "facepalm", "Please stop...")
                ]
            }},
            {"WethWentMissing_Multi_0", new(){
                type = NodeType.combat,
                allPresent = [AmMax],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmMax, "intense", "Hey? We need her!")
                ]
            }},
            {"WethWentMissing_Multi_1", new(){
                type = NodeType.combat,
                allPresent = [AmBooks],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmBooks, "intense", "Where did crystal lady go?")
                ]
            }},
            {"WethWentMissing_Multi_2", new(){
                type = NodeType.combat,
                allPresent = [AmDizzy],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmDizzy, "serious", "That's not good.")
                ]
            }},
            {"WethWentMissing_Multi_3", new(){
                type = NodeType.combat,
                allPresent = [AmIsaac],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmIsaac, "panic", "Weth!")
                ]
            }},
            {"WethWentMissing_Multi_4", new(){
                type = NodeType.combat,
                allPresent = [AmDrake],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmDrake, "panic", "Did she turn into a crystal?!")
                ]
            }},
            {"WethWentMissing_Multi_5", new(){
                type = NodeType.combat,
                allPresent = [AmPeri],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmPeri, "mad", "Hey! Bring her back!")
                ]
            }},
            {"WethWentMissing_Multi_6", new(){
                type = NodeType.combat,
                allPresent = [AmRiggs],
                lastTurnPlayerStatuses = [MissingWeth],
                priority = true,
                oncePerCombatTags = ["wethWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmRiggs, "serious", "Umm?")
                ]
            }},
        });
        LocalDB.DumpStoryToLocalLocale("en", "urufudoggo.Illeana", new Dictionary<string, DialogueMachine>()
        {
            {"IlleanaGotPerfect_Multi_0", new(){
                dialogue = [
                    new(),
                    new(AmWeth, "explain", "I'd actually rather not.")
                ]
            }},
        });
    }

    public static void LateRegister()
    {
        LocalDB.DumpStoryToLocalLocale("en", "urufudoggo.Illeana", new Dictionary<string, DialogueMachine>()
        {
            {"IlleanaWentMissing_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lastTurnPlayerStatuses = [AmIlleana.TryGetMissing()],
                priority = true,
                oncePerCombatTags = ["illeanaWentMissing"],
                oncePerRun = true,
                dialogue = [
                    new(AmWeth, "panic", "Oh no! The snake, she's gone!")
                ]
            }},
        });
    }
}
