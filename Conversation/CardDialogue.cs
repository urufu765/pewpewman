using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class CardDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"CATsummonedWethCard_Multi_0", new(){
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ AmCat ],
                lookup = [ "summonWeth" ],
                oncePerCombatTags = [ "summonWethTag" ],
                dialogue = [
                    new(AmCat, "We need more pew pew bang bang right about now.")
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
            }}
        });
    }
}