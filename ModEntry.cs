using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
//using Weth.Artifacts;
using Weth.Cards;
using Weth.External;
using Weth.Features;
using System.Reflection;
//using System.Reflection;

namespace Weth;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal static IPlayableCharacterEntryV2 WethTheSnep { get; private set; } = null!;
    internal string UniqueName { get; private set; }
    internal Harmony Harmony;
    internal IKokoroApi KokoroApi;
    internal IDeckEntry WethDeck;
    internal IDeckEntry GoodieDeck;
    public bool modDialogueInited;
    internal IStatusEntry PulseStatus { get; private set; } = null!;
    // internal ICardTraitEntry AutoSU { get; private set; } = null!;
    // internal Spr AutoSUSpr { get; private set; }
    //internal ICardTraitEntry AutoE { get; private set; } = null!;
    public Spr WethCommon { get; private set; }
    public Spr WethUncommon { get; private set; }
    public Spr WethRare { get; private set; }
    public Spr GoodieCrystal { get; private set; }
    public Spr GoodieCrystalA { get; private set; }
    public Spr GoodieMech { get; private set; }
    public Spr GoodieMechA { get; private set; }

    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi {get; private set; } = null!;

    /*
     * The following lists contain references to all types that will be registered to the game.
     * All cards and artifacts must be registered before they may be used in the game.
     * In theory only one collection could be used, containing all registerable types, but it is seperated this way for ease of organization.
     */
    private static List<Type> WethCommonCardTypes = [
        typeof(WethExe),
        typeof(TripleTap),
        typeof(Puckshot),
        //typeof(Splitshot),
        typeof(TrashDispenser),
        //typeof(CargoBlaster),
        typeof(Pulsedrive),
        //typeof(GiantTrash),
        //typeof(DoubleBlast),
        typeof(Overcompensator)
    ];
    private static List<Type> WethUncommonCardTypes = [
        typeof(DoubleTap),
        //typeof(Disabler),
        //typeof(ScatterTrash),
        //typeof(MegaTrash),
        typeof(Powershot),
        //typeof(Wideshot),
        typeof(Bloom)
    ];
    private static List<Type> WethRareCardTypes = [
        typeof(UnstoppableForce),
        typeof(PearlDispenser),
        typeof(CrisisCall),
        typeof(PowPow),
        typeof(ExtremeViolence)
    ];
    private static List<Type> WethSpecialCardTypes = [
        typeof(CryAhtack),
        typeof(CryDuhfend),
        typeof(CryDodge),
        typeof(CryEnergy),
        typeof(CryEvade),
        typeof(CryFlux),
        typeof(CryShield),
        typeof(CrySwap),
        typeof(MechAhtack),
        typeof(MechDuhfend),
        typeof(MechEvade),
        typeof(MechHull),
        typeof(MechMine),
        typeof(MechMissile),
        typeof(MechStun),
        typeof(MechSwap)
    ];
    private static IEnumerable<Type> WethCardTypes =
        WethCommonCardTypes
            .Concat(WethUncommonCardTypes)
            .Concat(WethRareCardTypes)
            .Concat(WethSpecialCardTypes);

    private static List<Type> WethCommonArtifacts = [
    ];
    private static List<Type> WethBossArtifacts = [
    ];
    private static List<Type> WethEventArtifacts = [
    ];
    private static IEnumerable<Type> WethArtifactTypes =
        WethCommonArtifacts
            .Concat(WethBossArtifacts)
            .Concat(WethEventArtifacts);

    private static IEnumerable<Type> AllRegisterableTypes =
        WethCardTypes
            .Concat(WethArtifactTypes);

    private static List<string> Weth1Anims = [
        "gameover",
        "mini",
        "placeholder"
    ];
    private static List<string> Weth3Anims = [
        //"down",
        //"tilt",
        //"tiltalt",
    ];
    private static List<string> Weth4Anims = [
        //"think",
    ];
    private static List<string> Weth5Anims = [
        //"explain",
        "neutral",
        //"sad",
        //"sparkle",
        "squint",
        //"up",
    ];
    private static List<string> Weth6Anims = [
        //"plead",
    ];
    public readonly static IEnumerable<string> WethAnims =
        Weth1Anims
            .Concat(Weth3Anims)
            .Concat(Weth4Anims)
            .Concat(Weth5Anims)
            .Concat(Weth6Anims);

    public static bool Patch_EnemyPack {get; private set;}


    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new Harmony("urufudoggo.Weth");
        UniqueName = package.Manifest.UniqueName;
        modDialogueInited = false;
        /*
         * Some mods provide an API, which can be requested from the ModRegistry.
         * The following is an example of a required dependency - the code would have unexpected errors if Kokoro was not present.
         * Dependencies can (and should) be defined within the nickel.json file, to ensure proper load mod load order.
         */
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");
        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
        {
            if (phase == ModLoadPhase.AfterDbInit)
            {
                Patch_EnemyPack = helper.ModRegistry.LoadedMods.ContainsKey("TheJazMaster.EnemyPack");
                DialogueMachine.Apply();
            }

        };

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        /*
         * A deck only defines how cards should be grouped, for things such as codex sorting and Second Opinions.
         * A character must be defined with a deck to allow the cards to be obtainable as a character's cards.
         */
        WethDeck = helper.Content.Decks.RegisterDeck("weth", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                /*
                 * This color is used in a few places:
                 * TODO On cards, it dictates the sheen on higher rarities, as well as influences the color of the energy cost.
                 * If this deck is given to a playable character, their name will be this color, and their mini will have this color as their border.
                 */
                color = new Color("125f66"),

                titleColor = new Color("93c4c8").addClarityBright()
            },

            DefaultCardArt = StableSpr.cards_MultiBlast,
            BorderSprite = RegisterSprite(package, "assets/frame_weth.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Weth", "name"]).Localize,
            ShineColorOverride = _ => new Color(0, 0, 0),
        });
        WethCommon = RegisterSprite(package, "assets/frame_wethcommon.png").Sprite;
        WethUncommon = RegisterSprite(package, "assets/frame_wethuncommon.png").Sprite;
        WethRare = RegisterSprite(package, "assets/frame_wethrare.png").Sprite;
        
        GoodieDeck = helper.Content.Decks.RegisterDeck("goodie", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                /*
                 * This color is used in a few places:
                 * TODO On cards, it dictates the sheen on higher rarities, as well as influences the color of the energy cost.
                 * If this deck is given to a playable character, their name will be this color, and their mini will have this color as their border.
                 */
                color = new Color("93c4c8"),

                titleColor = new Color("000000")
            },

            DefaultCardArt = StableSpr.cards_MultiBlast,
            BorderSprite = RegisterSprite(package, "assets/frame_goodies.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Goodie", "name"]).Localize,
            ShineColorOverride = _ => new Color(0, 0, 0),
        });
        GoodieCrystal = RegisterSprite(package, "assets/frame_goodiescrystal.png").Sprite;
        GoodieCrystalA = RegisterSprite(package, "assets/frame_goodiescrystalA.png").Sprite;
        GoodieMech = RegisterSprite(package, "assets/frame_goodiesmech.png").Sprite;
        GoodieMechA = RegisterSprite(package, "assets/frame_goodiesmechA.png").Sprite;

        /*
         * All the IRegisterable types placed into the static lists at the start of the class are initialized here.
         * This snippet invokes all of them, allowing them to register themselves with the package and helper.
         */
        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        
        /*
         * Characters have required animations, recommended animations, and you have the option to add more.
         * In addition, they must be registered before the character themselves is registered.
         * The game requires you to have a neutral animation and mini animation, used for normal gameplay and the map and run start screen, respectively.
         * The game uses the squint animation for the Extra-Planar Being and High-Pitched Static events, and the gameover animation while you are dying.
         * You may define any other animations, and they will only be used when explicitly referenced (such as dialogue).
         */
        foreach (string s1 in Weth1Anims)
        {
            RegisterAnimation(package, s1, $"assets/Animation/weth_{s1}", 1);
        }
        foreach (string s3 in Weth3Anims)
        {
            RegisterAnimation(package, s3, $"assets/Animation/weth_{s3}", 3);
        }
        foreach (string s4 in Weth4Anims)
        {
            RegisterAnimation(package, s4, $"assets/Animation/weth_{s4}", 4);
        }
        foreach (string s5 in Weth5Anims)
        {
            RegisterAnimation(package, s5, $"assets/Animation/weth_{s5}", 5);
        }
        foreach (string s6 in Weth6Anims)
        {
            RegisterAnimation(package, s6, $"assets/Animation/weth_{s6}", 6);
        }

        WethTheSnep = helper.Content.Characters.V2.RegisterPlayableCharacter("weth", new PlayableCharacterConfigurationV2
        {
            Deck = WethDeck.Deck,
            BorderSprite = RegisterSprite(package, "assets/char_frame_weth.png").Sprite,
            Starters = new StarterDeck
            {
                cards = [
                    new TripleTap(),
                    new Puckshot(),
                ],
                /*
                 * Some characters have starting artifacts, in addition to starting cards.
                 * This is where they would be added, much like their starter cards.
                 * This can be safely removed if you have no starting artifacts.
                 */
                artifacts = [
                ]
            },
            Description = AnyLocalizations.Bind(["character", "Weth", "desc"]).Localize
        });

        MoreDifficultiesApi?.RegisterAltStarters(WethDeck.Deck, new StarterDeck
        {
            cards = [
                new TrashDispenser(),
            ],
            artifacts = 
            [
            ]
        });

        /*
         * Statuses are used to achieve many mechanics.
         * However, statuses themselves do not contain any code - they just keep track of how much you have.
         */
        PulseStatus = helper.Content.Statuses.RegisterStatus("Pulsedrive", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("4ab3ff"),
                icon = ModEntry.RegisterSprite(package, "assets/pulsedrive.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "Pulsedrive", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Pulsedrive", "desc"]).Localize
        });
        // AutoSUSpr = RegisterSprite(package, "assets/autoplaysingle.png").Sprite;
        // AutoSU = helper.Content.Cards.RegisterTrait("AutoSU", new CardTraitConfiguration
        // {
        //     Name = AnyLocalizations.Bind(["trait", "Autousedestroy", "name"]).Localize,
        //     Tooltips = (state, card) =>
        //     {
        //         return [new GlossaryTooltip($"cardtrait.{MethodBase.GetCurrentMethod()!.DeclaringType!.Namespace!}::AutoSU")
        //         {
        //             Title = ModEntry.Instance.Localizations?.Localize(["trait", "Autousedestroy", "name"]),
        //             Description = ModEntry.Instance.Localizations?.Localize(["trait", "Autousedestroy", "desc"]),
        //             TitleColor = Colors.cardtrait,
        //             Icon = AutoSUSpr
        //         },
        //         new TTGlossary("cardtrait.singleUse")];
        //     },
        //     Icon = (state, card) => AutoSUSpr
        // });
        /*
         * Managers are typically made to register themselves when constructed.
         * _ = makes the compiler not complain about the fact that you are constructing something for seemingly no reason.
         */
        //_ = new KnowledgeManager();
        _ = new Pulsedriving();

        /*
         * Some classes require so little management that a manager may not be worth writing.
         * In AGainPonder's case, it is simply a need for two sprites and evaluation of an artifact's effect.
         */
        //AGainPonder.DrawSpr = RegisterSprite(package, "assets/ponder_draw.png").Sprite;
        //AGainPonder.DiscardSpr = RegisterSprite(package, "assets/ponder_discard.png").Sprite;
        //AOverthink.Spr = RegisterSprite(package, "assets/overthink.png").Sprite;

        // Artifact Section
        foreach (Type ta in WethArtifactTypes)
        {
            helper.Content.Artifacts.RegisterArtifact(ta.Name, UhDuhHundo.ArtifactRegistrationHelper(ta, RegisterSprite(package, "assets/Artifact/" + ta.Name + ".png").Sprite));
        }

        //DrawLoadingScreenFixer.Apply(Harmony);
        //SashaSportingSession.Apply(Harmony);
    }

    /*
     * assets must also be registered before they may be used.
     * Unlike cards and artifacts, however, they are very simple to register, and often do not need to be referenced in more than one place.
     * This utility method exists to easily register a sprite, but nothing prevents you from calling the method used yourself.
     */
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }


    /*
     * Animation frames are typically named very similarly, only differing by the number of the frame itself.
     * This utility method exists to easily register an animation.
     * It expects the animation to start at frame 0, up to frames - 1.
     * TODO It is advised to avoid animations consisting of 2 or 3 frames.
     */
    public static ICharacterAnimationEntryV2 RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    {
        return Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Instance.WethDeck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }
}

