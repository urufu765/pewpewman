using System.Linq;
using Nickel;

namespace Weth.Dialogue;

static class CommonDefinitions
{
    internal static ModEntry Instance => ModEntry.Instance;

    internal static string AmWeth => Instance.WethDeck.UniqueName;
    internal static Deck AmWethDeck => Instance.WethDeck.Deck;
    internal const string AmUnknown = "johndoe";
    internal const string AmCat = "comp";
    internal static string AmDizzy => Deck.dizzy.Key();
    internal static string AmPeri => Deck.peri.Key();
    internal static string AmRiggs => Deck.riggs.Key();
    internal static string AmDrake => Deck.eunice.Key();
    internal static string AmIssac => Deck.goat.Key();
    internal static string AmBooks => Deck.shard.Key();
    internal static string AmMax => Deck.hacker.Key();
    internal const string AmVoid = "void";
    internal const string AmShopkeeper = "nerd";

    internal static Status MissingWeth => ModEntry.WethTheSnep.MissingStatus.Status;
    internal static Status Pulsedrive => ModEntry.Instance.PulseStatus.Status;


    /// <summary>
    /// Safety checks if specific illeana animation exists, provides a placeholder if false
    /// </summary>
    /// <param name="loopTag">The Looptag of the animation</param>
    /// <returns>a valid looptag</returns>
    internal static string Check(this string loopTag)
    {
        if (ModEntry.WethAnims.Contains(loopTag))
        {
            return loopTag;
        }
        return "placeholder";
    }


    /// <summary>
    /// Converts the short name into the full name that the game will recognise
    /// </summary>
    /// <param name="Name">Name of artifact or item</param>
    /// <returns>Full name</returns>
    internal static string F(this string Name)
    {
        return $"{Instance.UniqueName}::{Name}";
    }
}