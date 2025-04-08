using System;
using Microsoft.Extensions.Logging;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal static partial class EventDialogue
{
    internal static void Inject()
    {
        EventExtend();
        Reply();
    }

    private static void EventExtend()
    {
    }
}