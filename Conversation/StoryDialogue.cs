using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class StoryDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.LocalStory.all["Weth_0"] = new DialogueMachine
        {
            type = NodeType.@event,
            lookup = ["zone_first"],
            bg = "BGRunStart",
            allPresent = [AmWeth],
            priority = true,
            dialogue = [
                new SayThing(AmWeth, "This is a dialogue test"),
                new SayThing(AmCat, "I'm responding!"),
                new SwitchThing([
                    new(AmRiggs, "Woah!"),
                    new(AmDizzy, "Yay"),
                ]),
                new SayThing(AmCat, "squint", "Is this really it?"),
                new SayThing(AmWeth, "explain", "Really for sure."),
                new SayThing(AmWeth, "What do you think of this simplicity?")
            ]
        };

        // LocalDB.LocalStory.all["Weth_Goes_To_The_Bathroom_0"] = new DialogueMachine
        // {
        //     type = NodeType.combat,
        //     allPresent = [AmWeth],
        //     oncePerCombat = true,
        //     dialogue = [
        //         new(AmWeth, "Hold on, I'm going to the loo."),
        //         new(DMod.switchsay),
        //         new(AmMax, "What? That's illegal!"),
        //         new(AmPeri, "squint", "The what?"),
        //         new(AmRiggs, "Bring back something from the gift shop!"),
        //         new(AmCat, "In the middle of battle?!")
        //     ]
        // };
    }
}