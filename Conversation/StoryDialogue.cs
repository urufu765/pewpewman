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
            once = true,
            dialogue = [
                new(AmWeth, "tired", "..."),
                new(AmCat, "Hey, who let the dogs in?"),
                new(AmWeth, "squint", "Where am I?"),
                new(AmCat, "A ship!"),
                new(AmWeth, "Fair nuff... where we going?"),
                new(AmCat, "To the Cobalt!"),
                new(AmWeth, "The Cobalt?!"),
                new(AmWeth, "sparkle", "Sign me right up!")
            ]
        };
    }
}