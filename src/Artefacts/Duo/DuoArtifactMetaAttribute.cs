using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

public class DuoArtifactMeta : Attribute
{
    public Deck duoDeck;
    public string? duoModDeck;
}