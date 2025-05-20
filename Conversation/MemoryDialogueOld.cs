using static Weth.Conversation.CommonDefinitions;

namespace Weth.Conversation;

internal static class MemoryDialoguee
{
    internal static void Inject()
    {
        DB.story.all["RunWinWho_Weth_1"] = new()
        {
            type = NodeType.@event,
            introDelay = false,
            allPresent = [ AmWeth ],
            bg = "BGRunWin",
            lookup = [
                $"runWin_{AmWeth}"
            ],
            lines = new()
            {
                new Wait
                {
                    secs = 3
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "sparkle".Check(),
                    what = "Ooh, mysterious presence!"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "Hello, little one."
                },
                new CustomSay
                {
                    who = AmWeth,
                    what = "Are you in my head?"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "No."
                },
                new CustomSay
                {
                    who = AmWeth,
                    what = "Are you a dragon?"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "No."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "curious".Check(),
                    what = "What are you?"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "Your last discovery."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "explain".Check(),
                    what = "I'll pretend I didn't hear that."
                },
            }
        };
        DB.story.all["RunWinWho_Weth_2"] = new()
        {
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
            lines = new()
            {
                new Wait
                {
                    secs = 3
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "neutral".Check(),
                    what = "So what happens to me now?"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "You will be consumed by the time crystal."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "sad".Check(),
                    what = "Oh... I see..."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "sad".Check(),
                    what = "Can I at least say good bye to my friends?"
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "Go ahead"
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "think".Check(),
                    what = "..."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "sad".Check(),
                    what = "I... I don't know how."
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "That's okay. You don't have to say anything."
                }

            }
        };
        DB.story.all["RunWinWho_Weth_3"] = new()
        {
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
            lines = new()
            {
                new Wait
                {
                    secs = 3
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "Your time is almost up."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "down".Check(),
                    what = "I'm not ready."
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "I don't expect you to be."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "up".Check(),
                    what = "I... I haven't done all the things I wanted to do."
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "You did what you could."
                },
                new CustomSay
                {
                    who = AmWeth,
                    loopTag = "crystalized".Check(),
                    what = "..."
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "..."
                },
                new CustomSay
                {
                    who = AmVoid,
                    flipped = true,
                    what = "Rest well, little one."
                }
            }
        };
    }
}