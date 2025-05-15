using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class MemoryDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"RunWinWho_Weth_1", new(){
                type = NodeType.@event,
                introDelay = false,
                allPresent = [ AmWeth ],
                bg = "BGRunWin",
                lookup = [
                    $"runWin_{AmWeth}"
                ],
                dialogue = [
                    new(new Wait{secs = 3}),
                    new(AmWeth, "sparkle", "Ooh! Mysterious presence!"),
                    new(AmVoid, "Hello, little one.", true),
                    new(AmWeth, "Are you inside my head?"),
                    new(AmVoid, "No.", true),
                    new(AmWeth, "Are you a space dragon?"),
                    new(AmVoid, "No.", true),
                    new(AmWeth, "squint", "What are you then?"),
                    new(AmVoid, "Your last discovery.", true),
                    new(AmWeth, "squint", "I'll pretend I didn't hear that.")
                ]
            }},
            {"RunWinWho_Weth_2", new(){
                type = NodeType.@event,
                introDelay = false,
                allPresent = [ AmWeth ],
                bg = "BGRunWin",
                lookup = [
                    $"runWin_{AmWeth}"
                ],
                requiredScenes = [
                    "RunWinWho_Weth_1"
                ],
                dialogue = [
                    new(new Wait{secs = 3}),
                    new(AmWeth, "So, what happens to me now?"),
                    new(AmVoid, "You will be consumed by the crystal.", true),
                    new(AmWeth, "Oh... I see..."),
                    new(AmWeth, "plead", "Can I at least say good bye to my new friends?"),
                    new(AmVoid, "Go ahead.", true),
                    new(AmWeth, "tired", "..."),
                    new(AmWeth, "sad", "I... I don't know how."),
                    new(AmVoid, "That's okay. You don't have to say anything.", true)
                ]
            }},
            {"RunWinWho_Weth_3", new(){
                type = NodeType.@event,
                introDelay = false,
                allPresent = [ AmWeth ],
                bg = "BGRunWin",
                lookup = [
                    $"runWin_{AmWeth}"
                ],
                requiredScenes = [
                    "RunWinWho_Weth_2"
                ],
                dialogue = [
                    new(new Wait{secs = 3}),
                    new(AmVoid, "Your time is almost up.", true),
                    new(AmWeth, "down", "I'm not ready."),
                    new(AmVoid, "I don't expect you to be.", true),
                    new(AmWeth, "up", "I... I haven't done all the things I wanted to do..."),
                    new(AmVoid, "You did what you could.", true),
                    new(AmWeth, "crystallized", "..."),
                    new(AmVoid, "...", true),
                    new(AmVoid, "Rest well, little one.", true)
                ]
            }},
            {$"{AmWeth}_Memory_1", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGVault",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                dialogue = [
                    new(""),
                    new(new Wait{secs = 2}),
                    new(DMod.title),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastneutral", "Here it is, the shipwreck yard."),
                    new(AmWeth, "pastsquint", "And no signs of looting or anything. Imagine all the relics I can collect from these!"),
                    new(AmWeth, "pastlockedin", "Hidden treasure, here I come!"),
                    new(new Wait{secs = 1}),
                    // Explosion
                    new(AmWeth, "pastsurprise", "..."),
                    new(AmWeth, "pastmad", "Dammit! Not again!"),
                    new(AmWeth, "pastsquint", "Where's that fire extinguisher?"),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastfacepalm", "Man, what was I thinking."),
                    new(AmWeth, "pasteyeroll", "Oh, I'll just install these giant drills to make excavation easier!"),
                    new(AmWeth, "pastdonewithit", "That won't constantly set the engines on fire!"),
                    new(new Wait{secs = 3}),
                    new(AmWeth, "pastmad", "..."),
                    new(AmWeth, "pastputoutfire", "Stop."),
                    new(AmWeth, "pastputoutfire", "Being."),
                    new(AmWeth, "pastputoutfire", "On."),
                    new(AmWeth, "pastputoutfire", "Fire!"),
                ]
            }},
            {$"{AmWeth}_Memory_2", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGVault",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                requiredScenes = [$"{AmWeth}_Memory_1"],
                dialogue = [
                    new(""),
                    new(new Wait{secs = 2}),
                    new(DMod.title),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastneutral", "So, can you fix it?"),
                    new(new Wait{secs = 1}),
                    new(AmShopkeeper, "Nope!", true),
                    new(AmWeth, "pastsurprise", "What?! Why not?"),
                    new(AmWeth, "pastsquint", "This isn't my worst wreckage yet..."),
                    new(AmWeth, "pastplead", "... I think..."),
                    new(AmShopkeeper, "I swear, you and your luck...", true),
                    new(AmShopkeeper , "You gotta stop doing this to your ship.", true),
                    new(AmWeth, "pastexplain", "Well the expeditions aren't gonna complete themselves."),
                    new(AmShopkeeper, "I don't think I've seen you complete an expedition without basically limping to the finish line.", true),
                    new(AmShopkeeper, "This is the second time just this week too.", true),
                    new(AmWeth, "pastsilly", "Aren't I your best customer?"),
                    new(AmShopkeeper, "...", true),
                    new(AmWeth, "pastsilly", "..."),
                    new(AmShopkeeper, "Find a better hobby.", true),
                    new(AmWeth, "pastplead", "Yes ma'am.")
                ]
            }},
            {$"{AmWeth}_Memory_3", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGVault",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                requiredScenes = [$"{AmWeth}_Memory_2"],
                dialogue = [
                    new(""),
                    new(new Wait{secs = 2}),
                    new(DMod.title),
                    new(new Wait{secs = 3}),
                    new(AmWeth, "pastsparkle", "Woah!"),
                    new(AmWeth, "pastsparkle", "Who knew the universe was this pretty?"),
                    new(new Wait{secs = 2}),
                    new(AmWeth, "pastneutral", "I was so focused on looking for treasure that I've missed the most important thing in a journey."),
                    new(AmWeth, "pastexplain", "The journey."),
                    new(AmWeth, "pasttired", "..."),
                    new(AmWeth, "pastsquint", "Is there a gasleak somewhere? Or was I always this delusional?"),
                    new(new Wait{secs = 2}),
                    new(AmWeth, "pastneutral", "Hey? What's that?"),
                    new(AmWeth, "pastsurprise", "Woah! That's a big ship!"),
                    new(AmWeth, "pastsquint", "And it's a design I've never seen before..."),
                    new(AmWeth, "pasthappy", "Aw man, this brings me way back when I was a youngin!"),
                    new(AmWeth, "pasthappy", "..."),
                    new(AmWeth, "pastlockedin", "They wouldn't mind if I get a closer look, right?"),
                    new(new Wait{secs = 2}),  // scene transition
                    new(AmWeth, "pastsparkle", "WOAH! It's even cooler close up!"),
                    new(AmWeth, "pastneutral", "I'm so glad I installed that stealth kit a while back..."),
                    new(AmWeth, "pastwait", "..."),
                    new(AmWeth, "pastsquint", "..."),
                    new(AmWeth, "pastsquint", "..."),
                    new(AmWeth, "pastexplain", "Okay it didn't catch on fire."),
                    // Explosion
                    new(AmWeth, "pastsurprise", "What was that?!"),
                    // crystal crash into Weth
                    new(AmWeth, "pastscream", "AAAAAAAAAAAAAAAAAGH!"),  // Clutching her left eye
                    new("")  // T-0s
                ]
            }},

        });
    }
}