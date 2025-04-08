using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal static partial class CombatDialogue
{
    internal static void Inject()
    {
        Replies();
        ModdedInject();
        MainExtensions();
    }


    private static void MainExtensions()
    {
    }
}

