using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;
using Weth.External;
using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

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
            {"Weth_Memory_1", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGWethCustomRings",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                dialogue = [
                    new("T-100 days"),
                    new(new Wait{secs = 2}),
                    new(title: null),  // Clears title card
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastneutral", "Here it is, the shipwreck yard."),
                    new(new Wait{secs = 2}),
                    new(AmWeth, "pastsquint", "And no signs of looting or anything."),
                    new(AmWeth, "pasthappy", "Imagine all the relics I can collect from these!"),
                    new(AmWeth, "pastlockedin", "Hidden treasure, here I come!"),
                    new(new Wait{secs = 2}),
                    new(new BGAction{action = "rumble_on"}),
                    new(new BGAction{action = "flash"}),
                    // Explosion
                    new(AmWeth, "pastsurprise", "..."),
                    new(AmWeth, "pastmad", "Dammit! Not again!"),
                    new(AmWeth, "pastlookfor", "Where's that fire extinguisher?"),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastfacepalm", "Man, what was I thinking."),
                    new(AmWeth, "pasteyeroll", "Oh, I'll just install these giant drills to make excavation easier!"),
                    new(AmWeth, "pastdonewithit", "That won't constantly set the engines on fire!"),
                    new(new BGAction{action = "flash_weak"}),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastmad", "..."),
                    new(new BGAction{action = "bonk"}),
                    new(AmWeth, "pastputoutfire", "STOP."),
                    new(new BGAction{action = "bonk"}),
                    new(AmWeth, "pastputoutfire", "BEING."),
                    new(new BGAction{action = "bonk"}),
                    new(AmWeth, "pastputoutfire", "ON."),
                    new(new BGAction{action = "bonk"}),
                    new(AmWeth, "pastputoutfire", "FIRE!"),
                ]
            }},
            {"Weth_Memory_2", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGWethShop",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                requiredScenes = ["Weth_Memory_1"],
                dialogue = [
                    new("T-14 days"),
                    new(new Wait{secs = 2}),
                    new(DMod.title),  // Also clears title card
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastneutral", "So, can you fix it?"),
                    new(new Wait{secs = 1}),
                    new(AmShopkeeper, "explains", "Nope!", true),
                    new(AmWeth, "pastsurprise", "What?! Why not?"),
                    new(AmWeth, "pastneutral", "This isn't my worst wreckage yet..."),
                    new(AmWeth, "pastsquint", "... I think..."),
                    new(AmShopkeeper, "I swear, you and your luck...", true),
                    new(AmShopkeeper , "You gotta stop doing this to your ship.", true),
                    new(AmWeth, "pastexplain", "Well the expeditions aren't gonna complete themselves."),
                    new(AmShopkeeper, "I don't think I've seen you complete an expedition without basically limping to the finish line.", true),
                    new([
                        new(AmShopkeeper, "nervous", "This is the second time just this week too.", true),
                        new(AmShopkeeper, "Your luck is gonna run out eventually.", true),
                        new(AmShopkeeper, "explains", "You're also straining my inventory.", true)
                    ]),
                    new(AmWeth, "pastsilly", "Aren't I your best customer?"),
                    new(AmShopkeeper, "...", true),
                    new([
                        new(AmWeth, "pastcheese", "..."),
                        new(AmWeth, "pastsilly", "..."),
                    ]),
                    new(AmShopkeeper, "Find a better hobby.", true),
                    new(AmWeth, "pastplead", "Yes ma'am.")
                ]
            }},
            {"Weth_Memory_3", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "BGWethVault",
                lookup = [
                    "vault", $"vault_{AmWeth}"
                ],
                requiredScenes = [$"Weth_Memory_2"],
                dialogue = [
                    new("T-10 minutes"),
                    new(new Wait{secs = 2}),
                    new(new TitleCard{empty = true}),  // Also ALSO clears title card
                    new(new Wait{secs = 3}),
                    new(AmWeth, "pastsparkle", "Woah!"),
                    new(AmWeth, "pastsparkle", "Who knew the universe was this pretty?"),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "pastneutral", "I can't believe I've failed to realize that I have been missing out on the most important aspect of discovery until now."),
                    new(AmWeth, "pastexplain", "The journey itself."),
                    new(AmWeth, "pasttired", "..."),
                    new(AmWeth, "pastsquint", "Is there a gasleak somewhere? Or was I always this delusional?"),
                    new(new BGAction{action = "peek"}),
                    new(new Wait{secs = 2}),
                    new(AmWeth, "pastneutral", "Hey? What's that?"),
                    new(AmWeth, "pastsurprise", "Woah! That's a big ship!"),
                    new(AmWeth, "pastsquint", "And it's a design I've never seen before..."),
                    new(AmWeth, "pasthappy", "Aw man, this brings me way back when I was a youngin' venturing the docks to look at ships!"),
                    new(AmWeth, "pasthappy", "..."),
                    new(AmWeth, "pasthappy", "They wouldn't mind if I get a closer look, right?"),
                    new(new BGAction{action = "transition"}),
                    new(new Wait{secs = 5}),  // scene transition
                    new(AmWeth, "pastsparkle", "WOAH! It's even cooler close up!"),
                    new(AmWeth, "pasthappy", "I'm so glad I installed that stealth kit a while back..."),
                    new(AmWeth, "pastwait", "..."),
                    new(new BGAction{action = "auto_advance_on"}),
                    new(AmWeth, "pastglare", "...", delay: 1),
                    new(AmWeth, "pastglareoffscreen", "...", delay: 1),
                    new(AmWeth, "pastnotpresent", "...", delay: 1),
                    new(AmWeth, "pastglareoffscreenextinguisher", "...", delay: 1),
                    new(new BGAction{action = "auto_advance_off"}),
                    new(AmWeth, "pastglarewithextinguisher", "..."),
                    new(AmWeth, "pastexhausted", "Okay good, it didn't catch on fire."),
                    // Explosion
                    new(new BGAction{action = "rumble_on"}),
                    new(AmWeth, "pastneutral", "Huh? What's happening?"),
                    // crystal crash into Weth
                    new(new BGAction{action = "rumble_intensify"}),
                    new(new Wait{secs = 0.7}),
                    new(new BGAction{action = "shard"}),
                    new(new Wait{secs = 0.3}),
                    new(AmWeth, "pastscream", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!"),  // Clutching her left eye
                    new(new BGAction{action = "stop"}),
                    new("<c=downside>T-0 seconds</c>"),  // T-0s
                    new(new Wait{secs = 9})
                ]
            }},
            {"Tarmauc_Memory_1", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "",
                lookup = [
                    "vault", $"vault_{AmTarmauc}"
                ],
                dialogue = [
                    new("T-??? days"),
                    new(new Wait{secs = 2}),
                ]
            }},
            {"Tarmauc_Memory_2", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "",
                lookup = [
                    "vault", $"vault_{AmTarmauc}"
                ],
                requiredScenes = [
                    "Tarmauc_Memory_1"
                ],
                dialogue = [
                    new("T-??? days"),
                    new(new Wait{secs = 2}),
                ]
            }},
            {"Tarmauc_Memory_2_End_01", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemoryLovers",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                    new(new BGAction{action = "VisorSuddenTakeOff"}),
                    new(new Wait{secs = 3}),
                    new(AmTarmauc, "blushshocked", "..."),
                    new(AmTarmauc, "blusheyesclosed", "..."),
                    new(new Wait{secs = 2}),
                    new(AmTarmauc, "blushlookup", "Agh! Stop THINKING about it!")
                ]
            }},
            {"Tarmauc_Memory_2_End_02", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemoryStrength",
                dialogue = [
                    new(AmTarmauc, "teenneutral", "Actually, before we say our goodbyes..."),
                    new(AmTarmauc, "teensmirk", "Why don't we have one last bout, to see who's the strongest?"),
                    new(AmWeth, "teensmug", "I like the sound of that.", flipped:true),
                    new(new BGAction{action = "LeapAwayFromEachOther"}),
                    new(new Wait{secs = 2}),
                    new(new BGAction{action = "SlideInTornyFighterUI"}),
                    new(new BGAction{action = "TextGetReadyTVIn"}),
                    new(new Wait{secs = 3}),
                    new(new BGAction{action = "TextGetReadyGone"}),
                    new(new Wait{secs = 1}),
                    new(new BGAction{action = "TextFightTVIn"}),
                    new(new Wait{secs = 1}),
                    new(new BGAction{action = "TextFightFadeOut"}),
                    new(new BGAction{action = "ChargeFist"}),
                    new(new Wait{secs = 2}),
                    new(new BGAction{action = "FistsFlying"}),
                    new(new Wait{secs = 3}),

                    new(new SetBG{bg = "BGTarmaucShipAfterStrength"}),
                    new(new BGAction{action = "PunchedAPole"}),  // Funny sfx?
                    new(new Wait{secs = 3}),
                    new(new BGAction{action = "CrouchHoldingHandInPain"}),
                    new(new Wait{secs = 4}),
                    new(new BGAction{action = "RaiseHeadAndStartScreaming"}),  // Autoadvance dialogue
                    new(AmTarmauc, "holdfistinagony", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")
                ]
            }},
            {"Tarmauc_Memory_2_End_03", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_04", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_05", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_06", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_07", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_08", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_09", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemory",
                dialogue = [
                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                ]
            }},
            {"Tarmauc_Memory_2_End_10", new(){
                type = NodeType.@event,
                bg = "BGPastTarmaucMemoryDeath",
                dialogue = [
                    new(AmTarmauc, "teenneutral", "See you on the other side."),
                    new(AmWeth, "teentongueout", "Blegh, what a cliche line.", flipped:true),
                    new(AmTarmauc, "teengrumpy", "As if you have something better."),
                    new(AmWeth, "teensmug", "Actually I do.", flipped:true),
                    new(new BGAction{action = "WethPunchesTarmaucInTheShoulder"}),
                    new(new Wait{secs = 3}),
                    new(AmWeth, "teensmileeyesclosed", "See you soon, Road Kill.", flipped: true),
                    new(AmTarmauc, "teensmirk", "That was equally terrible."),
                    new(AmWeth, "teensmug", "It's the thought that counts.", flipped:true),
                    new(new Wait{secs = 3}),
                    new(AmWeth, "teenlookdown", "Promise me you'll be back soon.", flipped:true),
                    new(AmTarmauc, "teenshrug", "That's not for me to decide you know."),
                    new(AmWeth, "teentears", "Just promise me.", flipped:true),
                    new(AmTarmauc, "teenwait", "Okay okay! Fine!"),
                    new(AmTarmauc, "teensmirk", "See you later, doggy."),
                    new(new Wait{secs = 1}),
                    new(AmWeth, "teenpout", "You're terrible at nicknames.", flipped:true),
                    new(AmTarmauc, "teenshrug", "I'll try to think of something better by the time I'm back."),
                    new(AmWeth, "teentongueout", "You better!", flipped:true),
                    new(new Wait{secs = 2}),

                    new(new SetBG{bg = "BGTarmaucShipAfter"}),
                    new(new BGAction{action = "VisorSlowlyTakeOff"}),
                    new(AmTarmauc, "This is it."),
                    new(AmTarmauc, "excited", "I found it!"),
                    new(AmTarmauc, "outofbounds", "Companion bot! Save positional energy data!"),
                    new(AmCompanionChunk, "positional data saved. i will notify you once the ship is aligned", flipped: true),
                    new(AmTarmauc, "suddenenlightenment", "Ah!"),
                    new(AmTarmauc, "Companion bot, is it possible to broadcast a message, except this time to that universe's you only?"),
                    new(AmCompanionChunk, "yes commander. please speak your message now", flipped: true),
                    new(new BGAction{action = "ClearThroat"}),
                    new(AmTarmauc, "Ahem."),
                    new(new BGAction{action = "RecordingPose"}),
                    new(AmTarmauc, "confidenteyesclosed", "Dear idiot,"),
                    new(AmTarmauc, "confidentlookatdistant", "If you're listening to this message, then that means I have probably found a way back."),
                    new(AmTarmauc, "confidentdevious", "I'm definitely coming for you this time."),
                    new(AmTarmauc, "smile", "All the best, Tarmauc."),
                    new(new Wait{secs = 1}),
                    new(new BGAction{action = "Standing"}),
                    new(new Wait{secs = 1}),
                    new(AmCompanionChunk, "message saved. i will broadcast the message as soon as alignment completes", flipped: true),
                    new(AmCompanionChunk, "would you like to attach \"prank-self-destruct-sequence\" like last time?", flipped: true),
                    new(AmTarmauc, "evil", "Yes."),
                    new(AmCompanionChunk, "added", flipped: true),
                    new(new Wait{secs = 2}),
                    new(new BGAction{action = "Sit"}),
                    new([
                        new(AmTarmauc, "mumble", "Just you wait, Weth. I'll be keeping that promise."),
                        new(AmTarmauc, "mumble", "I hope you haven't forgotten our promise, Weth."),
                        new(AmTarmauc, "mumble", "You better be alive still, I can't wait to see you again.")
                    ])

                ]
            }},
            {"Tarmauc_Memory_3", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "",
                lookup = [
                    "vault", $"vault_{AmTarmauc}"
                ],
                requiredScenes = [
                    "Tarmauc_Memory_2", "Tarmauc_Memory_2_End_10"
                ],
                dialogue = [
                    new("T+??? days"),
                    new(new Wait{secs = 2}),
                ]
            }},
        });
    }
}