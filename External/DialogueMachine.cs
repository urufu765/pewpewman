using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.External;


public enum DMod
{
    dialogue,
    switchsay,
    retain,
    instruction,
    title,
}
public enum EMod
{
    countFromStart,
    countFromEnd,
    findSwitchWithHash
}


public class AbstractThing
{
    public string? who;
    public string? loopTag;
    public string? what;
    public bool flipped;
    public bool ifCrew;
    public double delay;
    public string? choiceFunc;
}


public class EditThing : AbstractThing
{
    public int? switchNumber;
    public EMod searchConfig;
    public string? hashSearch;
    public EditThing(EMod searchConfig, int switchNumber, string who, string loopTag, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.searchConfig = searchConfig;
        this.who = who;
        this.loopTag = loopTag;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
        this.switchNumber = switchNumber;
    }
    public EditThing(EMod searchConfig, int switchNumber, string who, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.searchConfig = searchConfig;
        this.who = who;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
        this.switchNumber = switchNumber;
    }
    public EditThing(string hashToFind, string who, string loopTag, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.searchConfig = EMod.findSwitchWithHash;
        this.who = who;
        this.loopTag = loopTag;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
        this.hashSearch = hashToFind;
    }
    public EditThing(string hashToFind, string who, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.searchConfig = EMod.findSwitchWithHash;
        this.who = who;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
        this.hashSearch = hashToFind;
    }
}


/// <summary>
/// Ver0.5
/// </summary>
public class DialogueThing : AbstractThing
{
    public string? title;
    public List<DialogueThing>? saySwitch;
    public DMod mode;
    public Instruction? instruction;

    /// <summary>
    /// A dialogue with emotions and all
    /// </summary>
    /// <param name="who">Who speaketh?</param>
    /// <param name="loopTag">How emote?</param>
    /// <param name="what">What they sayeth?</param>
    public DialogueThing(string who, string loopTag, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.mode = DMod.dialogue;
        this.who = who;
        this.loopTag = loopTag;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
    }
    /// <summary>
    /// A dialogue with neutral emotion
    /// </summary>
    /// <param name="who">Who speaketh?</param>
    /// <param name="what">What they sayeth?</param>
    public DialogueThing(string who, string what, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null)
    {
        this.mode = DMod.dialogue;
        this.who = who;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
    }
    /// <summary>
    /// Adds a spacer that will allow the original text to fill in if the mod order is suboptimal. ONLY USED FOR ADDING TO EXISTING DIALOGUE.
    /// </summary>
    public DialogueThing()
    {
        this.mode = DMod.retain;
    }
    /// <summary>
    /// For adding any instructions unfulfilled by this dialogue thing
    /// </summary>
    /// <param name="instruction">Instructions to add</param>
    public DialogueThing(Instruction instruction)
    {
        this.mode = DMod.instruction;
        this.instruction = instruction;
    }
    /// <summary>
    /// For adding text to title cards
    /// </summary>
    /// <param name="title">The title to show (NULL for empty=true)</param>
    public DialogueThing(string? title)
    {
        this.mode = DMod.title;
        this.title = title;
    }

    /// <summary>
    /// Practically a SaySwitch. The list cannot contain anything but just dialogue.
    /// </summary>
    /// <param name="saySwitch">A list of Dialogue to go in 'ere</param>
    public DialogueThing(List<DialogueThing> saySwitch)
    {
        this.mode = DMod.switchsay;
        this.saySwitch = saySwitch;
    }

    /// <summary>
    /// For advanced stuff
    /// </summary>
    /// <param name="mode">Mode of dialogue</param>
    /// <param name="who">Whomst'd've</param>
    /// <param name="loopTag">Emotion</param>
    /// <param name="what">What they say?</param>
    /// <param name="flipped">Flipped to other side</param>
    /// <param name="ifCrew">???</param>
    /// <param name="delay">Delay</param>
    /// <param name="choiceFunc">Route choose</param>
    /// <param name="saySwitch">SaySwitch list</param>
    /// <param name="instruction">Custom instruction</param>
    /// <param name="title">Title</param>
    public DialogueThing(DMod mode, string? who = null, string? loopTag = null, string? what = null, bool flipped = false, bool ifCrew = false, double delay = 0.0, string? choiceFunc = null, List<DialogueThing>? saySwitch = null, Instruction? instruction = null, string? title = null)
    {
        this.mode = mode;
        this.who = who;
        this.loopTag = loopTag;
        this.what = what;
        this.flipped = flipped;
        this.ifCrew = ifCrew;
        this.delay = delay;
        this.choiceFunc = choiceFunc;
        this.saySwitch = saySwitch;
        this.instruction = instruction;
        this.title = title;
    }
}

public class DialogueMachine : StoryNode
{
    // public List<(string whoOrCommand, string? loopTag, string? what)> dialogue = null!;
    public List<EditThing> edit = null!;
    public List<DialogueThing> dialogue = null!;
    public void Convert()
    {
        if (edit is not null)  // Skips dialogue conversion if edits are available
        {
            foreach (EditThing e in edit)
            {
                lines.Add(e.searchConfig switch
                {
                    EMod.countFromStart => new InsertDialogueInSwitch
                    {
                        say = ConvertDialogueToSay(e),
                        whichSwitch = e.switchNumber
                    },
                    EMod.countFromEnd => new InsertDialogueInSwitch
                    {
                        say = ConvertDialogueToSay(e),
                        whichSwitch = e.switchNumber,
                        fromEnd = true
                    },
                    EMod.findSwitchWithHash => new InsertDialogueInSwitch
                    {
                        say = ConvertDialogueToSay(e),
                        whichHash = e.hashSearch
                    },                    
                    _ => new InsertDialogueInSwitch
                    {
                        say = ConvertDialogueToSay(e),
                    }
                });
            }
            return;
        }
        foreach (DialogueThing d in dialogue)
        {
            lines.Add(ConvertDialogueToLine(d));
        }
    }

    private static Say ConvertDialogueToSay(AbstractThing at)
    {
        return new Say
        {
            who = at.who ?? "",
            loopTag = at.loopTag,
            hash = at.what ?? "",
            flipped = at.flipped,
            ifCrew = at.ifCrew,
            delay = at.delay,
            choiceFunc = at.choiceFunc
        };
    }

    private static Instruction ConvertDialogueToLine(DialogueThing dt)
    {
        if (dt.mode == DMod.retain)
        {
            return new RetainOrig();
        }
        if (dt.mode == DMod.instruction && dt.instruction is not null)
        {
            return dt.instruction;
        }
        if (dt.mode == DMod.title)
        {
            if (dt.title is null)
            {
                return new TitleCard { empty = true };
            }
            else
            {
                return new TitleCard { hash = dt.title };
            }
        }
        if (dt.mode == DMod.switchsay && dt.saySwitch is not null)
        {
            SaySwitch ss = new()
            {
                lines = new()
            };
            foreach (DialogueThing dial in dt.saySwitch)
            {
                if (dial.mode == DMod.dialogue) ss.lines.Add(ConvertDialogueToSay(dial));
            }
            return ss;
        }

        return ConvertDialogueToSay(dt);
    }
    
    /// <summary>
    /// Eventually find a better way of tackling this
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool DeckExists(string name)
    {
        if (ModEntry.Instance.Helper.Content?.Decks?.LookupByUniqueName(name) is not null)
        {
            return true;
        }
        if (ModEntry.Instance.Helper.Content?.Decks?.LookupByUniqueName($"{ModEntry.Instance.UniqueName}::{name}") is not null)
        {
            return true;
        }
        return false;
    }
}


/// <summary>
/// Puts a placeholder for the original dialogue you're editing to fill in in that very spot.
/// </summary>
public class RetainOrig : Instruction
{
}

public class InsertDialogueInSwitch : Instruction
{
    public Say say = null!;
    public int? whichSwitch;
    public bool fromEnd;
    public string? whichHash;
}

public class LocalDB
{
    public static Story LocalStory { get; set; } = new();
    public static Dictionary<string, Story> LocalStoryLocale { get; set; } = new();
    private static Story ToUseStory { get; set; } = new();
    public int incrementingHash = 1;
    public Dictionary<string, string> customLocalisation { get; private set; }
    //internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }

    public LocalDB(IPluginPackage<IModManifest> package)
    {
        customLocalisation = new();
        if (LocalStoryLocale.ContainsKey(DB.currentLocale.locale))  // For other coded translated dialogues
        {
            ModEntry.Instance.Logger.LogInformation("1");
            ToUseStory = LocalStoryLocale[DB.currentLocale.locale];
        }
        else if (File.Exists($"{package.PackageRoot}\\i18n\\{DB.currentLocale.locale}_story.json"))  // For i18n translated story dialogue
        {
            ToUseStory = Mutil.LoadJsonFile<Story>(package.PackageRoot.GetRelativeFile($"i18n/{DB.currentLocale.locale}_story.json").FullName);
        }
        else  // For default
        {
            ToUseStory = LocalStory;
        }
        PasteToDB(ToUseStory, DB.story);
    }

    public Dictionary<string, string> GetLocalizationResults()
    {
        return customLocalisation;
    }

    public void PasteToDB(Story from, Story to)
    {
        foreach (KeyValuePair<string, StoryNode> sn in from.all)
        {
            bool editMode = false;
            // Convert all custom DialogueThings from DialogueMachine to StoryNode lines
            if (sn.Value is DialogueMachine dm)
            {
                dm.Convert();
                editMode = dm.edit is not null;
            }

            if (editMode)
            {
                to.all[sn.Key] = InjectALineIn(sn.Value, to.all[sn.Key], sn.Key);
                continue;
            }


            // Copy storynodes from from to to
            if (to.all.ContainsKey(sn.Key))
            {
                to.all[sn.Key] = StitchNodesTogether(sn.Value, to.all[sn.Key], sn.Key);
            }
            else
            {
                for (int a = 0; a < sn.Value.lines.Count; a++)
                {
                    MakeLinesRecognisable(sn.Value.lines[a], sn.Key);
                }
                to.all.Add(sn.Key, sn.Value);
            }
        }
    }

    /// <summary>
    /// Safely inject a dialogue in an existing dialogue tree
    /// </summary>
    /// <param name="newStory"></param>
    /// <param name="existingStory"></param>
    /// <param name="script"></param>
    /// <returns></returns>
    private StoryNode InjectALineIn(in StoryNode newStory, in StoryNode existingStory, string script)
    {
        try
        {
            StoryNode result = existingStory;
            if (result.lines is not null)
            {
                foreach (Instruction instruction in newStory.lines)
                {
                    if (instruction is InsertDialogueInSwitch idis)
                    {
                        for (int a = 0, b = 0, c = result.lines.Count - 1; b < result.lines.Count && c > 0; b++, c--)
                        {
                            if (!idis.fromEnd && result.lines[b] is SaySwitch ss)
                            {
                                a++;
                                if (idis.whichHash is not null)
                                {
                                    foreach (Say say in ss.lines)
                                    {
                                        if (say.hash == idis.whichHash)
                                        {
                                            ss.lines.Add(GetSayFromIDIS(idis, script));
                                            break;
                                        }
                                    }
                                }
                                else if (idis.whichSwitch is not null && a == idis.whichSwitch)
                                {
                                    ss.lines.Add(GetSayFromIDIS(idis, script));
                                    break;
                                }
                            }
                            else if (idis.fromEnd && result.lines[c] is SaySwitch bs)
                            {
                                a++;
                                if (idis.whichSwitch is not null && a == idis.whichSwitch)
                                {
                                    bs.lines.Add(GetSayFromIDIS(idis, script));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Failed to edit a line with key:" + script);
            return existingStory;
        }
    }


    private Say GetSayFromIDIS(InsertDialogueInSwitch idis, string script)
    {
        string what = idis.say.hash;
        idis.say.hash = $"{GetType().FullName}:{incrementingHash++}";
        customLocalisation[$"{script}:{idis.say.hash}"] = what;
        return idis.say;
    }


    /// <summary>
    /// Combines two storynodes together, automatically determining which of the two are original text based on the existence of RetainOrig
    /// </summary>
    /// <param name="newStory"></param>
    /// <param name="existingStory"></param>
    /// <param name="script"></param>
    /// <returns></returns>
    public StoryNode StitchNodesTogether(in StoryNode newStory, in StoryNode existingStory, string script)
    {
        try
        {
            StoryNode result = new();
            if (existingStory.lines is not null)
            {
                bool newIsOriginal = false;

                // Check which node is the original
                for (int w = 0; w < existingStory.lines.Count; w++)
                {
                    if (existingStory.lines[w] is RetainOrig)
                    {
                        newIsOriginal = true;
                    }
                }

                result = newIsOriginal ? newStory : existingStory;
                StoryNode start = newIsOriginal ? existingStory : newStory;

                for (int x = 0; x < result.lines.Count && x < start.lines.Count; x++)
                {
                    if (result.lines[x] is RetainOrig)
                    {
                        MakeLinesRecognisable(start.lines[x], script);
                        result.lines[x] = start.lines[x];
                    }
                    else if (result.lines[x] is Say or SaySwitch)
                    {
                        result.lines[x] = CombineTwoSays(result.lines[x], start.lines[x], script);
                    }
                }

                if (start.lines.Count > result.lines.Count)
                {
                    for (int y = result.lines.Count; y < start.lines.Count; y++)
                    {
                        MakeLinesRecognisable(start.lines[y], script);
                        result.lines.Add(start.lines[y]);
                    }
                }
            }
            return result;
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Failed to edit a line with key:" + script);
            return existingStory;
        }
    }

    public SaySwitch CombineTwoSays(Instruction existingLine, Instruction newLine, string script)
    {
        SaySwitch result = new SaySwitch
        {
            lines = new()
        };
        if (existingLine is Say sayA)
        {
            result.lines.Add(sayA);
        }
        else if (existingLine is SaySwitch saySA)
        {
            result = saySA;
        }

        if (newLine is Say sayB)
        {
            MakeLinesRecognisable(sayB, script);
            result.lines.Add(sayB);
        }
        else if (newLine is SaySwitch saySB)
        {
            MakeLinesRecognisable(saySB, script);
            foreach (Say s in saySB.lines)
            {
                result.lines.Add(s);
            }
        }
        return result;
    }

    public void MakeLinesRecognisable(Instruction instruction, string script)
    {
        if (instruction is Say say)
        {
            string what = say.hash;
            say.hash = $"{GetType().FullName}:{incrementingHash++}";
            //say.who = TryDeckLookup(say.who);
            customLocalisation[$"{script}:{say.hash}"] = what;
        }
        else if (instruction is SaySwitch saySwitch)
        {
            for (int a = 0; a < saySwitch.lines.Count; a++)
            {
                MakeLinesRecognisable(saySwitch.lines[a], script);
            }
        }
        else if (instruction is TitleCard title)
        {
            string what = title.hash;
            title.hash = $"{GetType().FullName}:{incrementingHash++}";
            if (title.empty is not true)
            {
                customLocalisation[$"{script}:{title.hash}"] = what;
            }
        }
    }
}