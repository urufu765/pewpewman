using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class EventDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.LocalStory.all["AbandonedShipyard_Repaired"] = new DialogueMachine
        {
            edit = [
                new("7657a54e", AmWeth, "Got some cool samples!")
            ]
        };
    }
}
