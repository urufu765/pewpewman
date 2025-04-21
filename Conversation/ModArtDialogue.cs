using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class ModdedArtifactDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        // LocalDB.LocalStory.all["ArtifactJetThrustersNoIlleana_Weth_0"] = new DialogueMachine
        // {
        //     type = NodeType.combat,
        //     hasArtifacts = [""],
        //     allPresent = [AmWeth],
        //     dialogue = [
        //         new(AmWeth, "")
        //     ]
        // };


    }
}