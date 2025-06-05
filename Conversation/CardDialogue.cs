using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

internal class CardDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"CATsummonedWethCard_Multi_0", new(){
                type = NodeType.combat,
                allPresent = [AmCat],
                lookup = ["summonWeth"],
                oncePerCombatTags = ["summonWethTag"],
                oncePerRun = true,
                dialogue = [
                    new(AmCat, "squint", "Why do I suddenly feel like hoarding junk?")
                ]
            }},
            {"CATsummonedWethCard_Multi_1", new(){
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ AmCat ],
                lookup = [ "summonWeth" ],
                oncePerCombatTags = [ "summonWethTag" ],
                dialogue = [
                    new(AmCat, "We need more pew pew bang bang right about now.")
                ]
            }},
            { "MilkSodaBoom_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaBoom"],
                dialogue = [
                    new(AmWeth, "sodaexplode", "...")
                ]
            }},
            {"MilkSodaDrink_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaDrink"],
                dialogue = [
                    new(AmWeth, "sodadrink", "...")
                ]
            }},
            {"MilkSodaShakeGone_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaGone"],
                dialogue = [
                    new(AmWeth, "sodagone", "...")
                ]
            }},
            {"MilkSodaShakeUp_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaUp"],
                dialogue = [
                    new(AmWeth, "sodashakeup", "...")
                ]
            }},
            {"MilkSodaShakeDown_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaDown"],
                dialogue = [
                    new(AmWeth, "sodashakedown", "...")
                ]
            }},
            {"MilkSodaExplodeUp_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaBoomUp"],
                dialogue = [
                    new(AmWeth, "sodaexplodeup", "...")
                ]
            }},
            {"MilkSodaExplodeDown_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["shakeSodaBoomDown"],
                dialogue = [
                    new(AmWeth, "sodaexplodedown", "...")
                ]
            }},
            {"FeralCardKill_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["feralCardUnflipped"],
                dialogue = [
                    new(AmWeth, "feralkill", "KILL!")
                ]
            }},
            {"FeralCardDie_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["feralCardFlipped"],
                dialogue = [
                    new(AmWeth, "feraldie", "DIE!")
                ]
            }},
            {"CrisisCallCardYeeted_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["crisisCallCulled"],
                dialogue = [
                    new([
                        new(AmWeth, "dontcare", "Oh no. The receiver. It blew."),
                        new(AmWeth, "dontcare", "We didn't need a receiver to call in extra firepower anyways."),
                        new(AmWeth, "dontcare", "At least I salvaged the useful components.")
                    ]),
                ]
            }},
            {"PowPowCardYeeted_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["powPowPopped"],
                dialogue = [
                    new([
                        new(AmWeth, "dontcare", "Whoops, I pushed it too far."),
                        new(AmWeth, "dontcare", "We got something out of it at least."),
                        new(AmWeth, "dontcare", "I'm sure we'll come across another one.")
                    ])
                ]
            }},
            {"AllPowerToCannonsCardPlayed_Weth_0", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["APTCcard"],
                dialogue = [
                    new(AmWeth, "lockedin", "Here we go!")
                ]
            }},
            {"AllPowerToCannonsCardPlayed_Weth_1", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["APTCcard"],
                dialogue = [
                    new(AmWeth, "lockedin", "Time to rain death!")
                ]
            }},
            {"AllPowerToCannonsCardPlayed_Weth_2", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["APTCcard"],
                dialogue = [
                    new(AmWeth, "lockedin", "Now or never!")
                ]
            }},
            {"AllPowerToCannonsCardPlayed_Weth_3", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["APTCcard"],
                dialogue = [
                    new(AmWeth, "lockedin", "I'll make it quick.")
                ]
            }},
            {"AllPowerToCannonsCardPlayed_Weth_4", new(){
                type = NodeType.combat,
                allPresent = [AmWeth],
                lookup = ["APTCcard"],
                dialogue = [
                    new(AmWeth, "lockedin", "I won't let this go to waste.")
                ]
            }},
        });
    }
}