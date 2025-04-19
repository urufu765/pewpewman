using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class EventDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        // LocalDB.LocalStory.all["DraculaTime"] = new DialogueMachine
        // {
        //     dialogue = [
        //         new(),
        //         new(),
        //         new(AmWeth, "Weh! WEH!")
        //     ]
        // };

        LocalDB.LocalStory.all["Pirate_1"] = new DialogueMachine
        {
            edit = [
                new(EMod.countFromStart, 2, AmWeth, "I'm here in the second switch!"),
                new(EMod.countFromEnd, 2, AmWeth, "Here I am, at the first switch!"),
            ]
        };
    }
}
// Dracula, Johnson, Pilot