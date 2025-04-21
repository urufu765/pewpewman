using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using Weth.External;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

internal class StoryDialogue : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"Weth_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = [AmWeth],
                priority = true,
                once = true,
                dialogue = [
                    new(AmWeth, "tired", "..."),
                    new(AmCat, "Hey, who let the dogs in?"),
                    new(AmWeth, "squint", "Where am I?"),
                    new(AmCat, "A ship!"),
                    new(AmWeth, "Fair nuff... where we going?"),
                    new(AmCat, "To the Cobalt!"),
                    new(AmWeth, "The Cobalt?!"),
                    new(AmWeth, "sparkle", "Sign me right up!")
                ]
            }},
            {"Weth_Peri_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = [AmWeth, AmPeri],
                priority = true,
                once = true,
                dialogue = [
                    new(AmWeth, "tired", "..."),
                    new(AmPeri, "squint", "..."),
                    new(AmWeth, "squint", "..."),
                    new(AmPeri, "squint", "You're not on the ship manifest."),
                    new(AmWeth, "squint", "I have no idea what that is."),
                    new(AmPeri, "squint", "..."),
                    new(AmWeth, "squint", "..."),
                    new(AmPeri, "squint", "..."),
                    new(AmWeth, "tired", "Okay my eyes are starting to hurt from all that squinting."),
                    new(AmWeth, "Move over."),
                    new(AmPeri, "What? Hey! What are you doing?!"),
                    new(AmWeth, "Manning the guns? What are YOU doing?"),
                    new(AmPeri, "You're in my seat."),
                    new(AmWeth, "How about we discuss the seating problems... after we blow up that ship that's in front of us?"),
                    new(AmPeri, "squint", "..."),
                    new(AmPeri, "squint", "Fine.")
                ]
            }},
            {"Weth_Peri_2", new(){
                type = NodeType.@event,
                lookup = ["after_crystal"],
                bg = "BGCrystalNebula",
                allPresent = [AmWeth, AmPeri],
                once = true,
                priority = true,
                requiredScenes = ["Weth_Peri_1"],
                dialogue = [
                    new(AmPeri, "Time's up."),
                    new(AmWeth, "What's up?"),
                    new(AmPeri, "Get out of my seat."),
                    new(AmWeth, "But I like this spot..."),
                    new(AmPeri, "mad", "Off."),
                    new(AmWeth, "sad", "..."),
                    new(AmPeri, "mad", "..."),
                    new(AmWeth, "sad", "..."),
                    new(AmPeri, "squint", "Fine, it's yours."),
                    new(AmWeth, "yay", "Yay!")
                ]
            }},
            {"Weth_Dizzy_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = [AmWeth, AmDizzy],
                priority = true,
                once = true,
                dialogue = [
                    new(AmWeth, "tired", "..."),
                    new(AmWeth, "..."),
                    new(AmWeth, "squint", "What are you doing?"),
                    new(AmDizzy, "You have a crystal growing out of your eye."),
                    new(AmWeth, "touch", "..."),
                    new(AmWeth, "Wow, you're right."),
                    new(AmDizzy, "Can I break a chunk off?"),
                    new(AmWeth, "What?"),
                    new(AmDizzy, "Can I break a chunk off your eye crystal?"),
                    new(AmWeth, "That sounds incredibly painful..."),
                    new(AmDizzy, "So that's a no?"),
                    new(AmWeth, "It's a maybe.")
                ]
            }},
            {"Weth_Dizzy_2", new(){
                type = NodeType.@event,
                lookup = ["after_crystal"],
                bg = "BGCrystalNebula",
                allPresent = [AmWeth, AmDizzy],
                once = true,
                requiredScenes = ["Weth_Dizzy_1", "PostCrystal_1"],
                dialogue = [
                    new(AmWeth, "What do you need my eye crystal for anyways?"),
                    new(AmDizzy, "Science!"),
                    new(AmWeth, "What kind of science?"),
                    new(AmDizzy, "I don't know!"),
                    new(AmWeth, "squint", "..."),
                    new(AmWeth, "squint", "I change my mind, you can't have it."),
                    new(AmDizzy, "squint", "Drat.")
                ]
            }},
            {"Weth_Dizzy_3", new(){
                type = NodeType.@event,
                lookup = ["after_crystal"],
                bg = "BGCrystalNebula",
                allPresent = [AmWeth, AmDizzy],
                once = true,
                requiredScenes = ["Weth_Dizzy_2", "PostCrystal_1"],
                dialogue = [
                    new(AmDizzy, "crystal", "..."),
                    new(AmWeth, "squint", "..."),
                    new(AmWeth, "squint", "Why are you holding that crystal chunk next to my head?"),
                    new(AmDizzy, "Science!"),
                    new(AmWeth, "I'd like to inform you it's also hurting me."),
                    new(AmDizzy, "crystal", "How so?"),
                    new(AmWeth, "tired", "I have a massive headache, my ears are ringing, and the area around my left eye is aching like crazy."),
                    new(AmDizzy, "crystal", "Interesting..."),
                    new(AmWeth, "squint", "Please take that away from me, before I violently throw you across the room."),
                    new(AmDizzy, "intense", "Okay.")
                ]
            }},
        });
    }
}