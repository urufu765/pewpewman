using System;
using Weth.Artifacts;
using Microsoft.Extensions.Logging;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal static partial class ArtifactDialogue
{
    internal static void Inject()
    {
        MainInjects();
        Replies();
    }

    private static void MainInjects()
    {
    }
}