using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Weth.Artifacts;
using Weth.Cards;
using Weth.External;
using Weth.Features;
using System.Reflection;
using Weth.Actions;
using Weth.Dialogue;
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
    internal IStatusEntry UnknownStatus { get; private set; } = null!;
    internal ISoundEntry JauntSlapSound { get; private set; } = null!;
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

    public Spr SprArtTHDepleted { get; private set; }

    public Spr SprArtTermMileCommon { get; private set; }
    public Spr SprArtTermMileBoss { get; private set; }
    public Spr SprArtTermMileRelic { get; private set; }

    public Spr SprArtTermJActive { get; private set; }
    public Spr SprArtTermJInactive { get; private set; }
    public Spr SprArtTermJReward { get; private set; }
    public Spr SprArtTermJAltReward { get; private set; }

    public Spr SprSplitshot { get; private set; }
    public Spr SprSplitshotFail { get; private set; }
    public Spr SprSplitshotPiercing { get; private set; }
    public Spr SprSplitshotPiercingFail { get; private set; }

    public Spr SprBayBlast { get; private set; }
    public Spr SprBayBlastFail { get; private set; }
    public Spr SprBayBlastWide { get; private set; }
    public Spr SprBayBlastWideFail { get; private set; }

    public Spr SprGiantAsteroidIcon { get; private set; }
    public Spr SprGiantAsteroid { get; private set; }
    public Spr SprMegaAsteroidIcon { get; private set; }
    public Spr SprMegaAsteroid { get; private set; }

    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi {get; private set; } = null!;
    internal IDuoArtifactsApi? DuoArtifactsApi {get; private set;} = null!;
    public LocalDB localDB { get; set; } = null!;

    /*
     * The following lists contain references to all types that will be registered to the game.
     * All cards and artifacts must be registered before they may be used in the game.
     * In theory only one collection could be used, containing all registerable types, but it is seperated this way for ease of organization.
     */
    private static List<Type> WethCommonCardTypes = [
        typeof(WethExe),
        typeof(TripleTap),
        typeof(Puckshot),
        typeof(SplitshotCard),
        typeof(TrashDispenser),
        typeof(CargoBlaster),
        typeof(PulsedriveCard),
        typeof(GiantTrash),
        typeof(DoubleBlast),
        typeof(Overcompensator)
    ];
    private static List<Type> WethUncommonCardTypes = [
        typeof(DoubleTap),
        typeof(Disabler),
        typeof(ScatterTrash),
        typeof(Discovery),
        typeof(Powershot),
        typeof(Spreadshot),
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
        typeof(MechSwap),
        typeof(CryPlaceholder),
        typeof(MechPlaceholder)
    ];
    private static IEnumerable<Type> WethCardTypes =
        WethCommonCardTypes
            .Concat(WethUncommonCardTypes)
            .Concat(WethRareCardTypes)
            .Concat(WethSpecialCardTypes);

    private static List<Type> WethCommonArtifacts = [
        typeof(TreasureSeeker),
        typeof(HiddenOptions),
    ];
    private static List<Type> WethBossArtifacts = [
        typeof(HiddenOptions2),
        typeof(TerminusMilestone),
        typeof(TerminusJaunt)
    ];
    private static List<Type> WethEventArtifacts = [
        typeof(TreasureHunter),
        typeof(SpaceRelics)
    ];
    private static List<Type> WethSpecialArtifacts = [
        typeof(RelicPulsedrive),
        typeof(RelicAutododgeRight),
        typeof(RelicBoost),
        typeof(RelicDrawNextTurn),
        typeof(RelicDroneShift),
        typeof(RelicEnergyFragment),
        typeof(RelicEvade),
        typeof(RelicFlux),
        typeof(RelicHermes),
        typeof(RelicShard),
        typeof(RelicShield),
        typeof(RelicStunCharge),
        typeof(RelicTempPayback),
        typeof(RelicTempShield)
    ];
    private static List<Type> WethDuoArtifacts = [
        typeof(CannonRecharge),  // Dizzy
        typeof(ResidualShot),  // Peri
        typeof(RockPower),  // Isaac
    ];
    private static IEnumerable<Type> WethArtifactTypes =
        WethCommonArtifacts
            .Concat(WethBossArtifacts)
            .Concat(WethEventArtifacts)
            .Concat(WethSpecialArtifacts)
            .Concat(WethDuoArtifacts);

    private static List<Type> WethDialogues = [
        typeof(StoryDialogue),
        typeof(EventDialogue),
        typeof(CombatDialogue),
        typeof(ArtifactDialogue)
    ];
    private static IEnumerable<Type> AllRegisterableTypes =
        WethCardTypes
            .Concat(WethDialogues);

    private static List<string> Weth1Anims = [
        //"crystallized", MEMORY ONLY
        "gameover",
        "mini",
        "placeholder",
        "traumatised",
    ];
    private static List<string> Weth3Anims = [
        "cryingcat",
        "crystal",
        //"down",  (mumbling, crystallization advances) MEMORY ONLY
        "facepalm",
        "lockedin",
        "mad",
        "touch",
        "yay",
    ];
    private static List<string> Weth4Anims = [
        "pain",
        //"think",  (Thinkin man pose) MEMORY ONLY
    ];
    private static List<string> Weth5Anims = [
        "apple",
        "explain",
        "neutral",
        "panic",
        "plead",
        "sad",
        "sparkle",
        "squint",
        "tired",
        //"up",  (Baby doll eyes, almost crystallized) MEMORY ONLY
    ];
    private static List<string> Weth6Anims = [
        //"maniac",  (ace attorney big evil dude from game 1, including the clapping)
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
        DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts");
        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
        {
            if (phase == ModLoadPhase.AfterDbInit)
            {
                if (DuoArtifactsApi is not null)
                {
                    foreach (Type type in WethDuoArtifacts)
                    {
                        DuoArtifactMeta dam = type.GetCustomAttribute<DuoArtifactMeta>()?? new DuoArtifactMeta();
                        if (dam.duoModDeck is null)
                        {
                            DuoArtifactsApi.RegisterDuoArtifact(type, [WethDeck!.Deck, dam.duoDeck]);
                        }
                        else
                        {
                            DuoArtifactsApi.RegisterDuoArtifact(type, [WethDeck!.Deck, helper.Content.Decks.LookupByUniqueName(dam.duoModDeck)!.Deck]);
                        }
                    }
                }
                Patch_EnemyPack = helper.ModRegistry.LoadedMods.ContainsKey("TheJazMaster.EnemyPack");
                localDB = new(helper, package);
            }
        };
        helper.Events.OnLoadStringsForLocale += (_, thing) =>
        {
            foreach (KeyValuePair<string, string> entry in localDB.GetLocalizationResults())
            {
                thing.Localizations[entry.Key] = entry.Value;
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
                color = new Color("2a767d"),

                titleColor = new Color("93c4c8").addClarityBright()
            },

            DefaultCardArt = StableSpr.cards_Cannon,
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

                titleColor = new Color("000000"),
            },

            DefaultCardArt = StableSpr.cards_MultiBlast,
            BorderSprite = RegisterSprite(package, "assets/frame_goodies.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Goodie", "name"]).Localize,
            //ShineColorOverride = _ => new Color(0, 0, 0),
        });
        GoodieCrystal = RegisterSprite(package, "assets/frame_goodiescrystal.png").Sprite;
        GoodieCrystalA = RegisterSprite(package, "assets/frame_goodiescrystalA.png").Sprite;
        GoodieMech = RegisterSprite(package, "assets/frame_goodiesmech.png").Sprite;
        GoodieMechA = RegisterSprite(package, "assets/frame_goodiesmechA.png").Sprite;

        
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
                    new TrashDispenser(),
                    new SplitshotCard()
                ],
                /*
                 * Some characters have starting artifacts, in addition to starting cards.
                 * This is where they would be added, much like their starter cards.
                 * This can be safely removed if you have no starting artifacts.
                 */
                artifacts = [
                    new TreasureHunter()
                ]
            },
            Description = AnyLocalizations.Bind(["character", "Weth", "desc"]).Localize,
            SoloStarters = new StarterDeck
            {
                cards = [
                    new Bloom(),
                    new DodgeColorless(),
                    new TrashDispenser(),
                    new GiantTrash(),
                    new SplitshotCard(),
                    new SplitshotCard()
                ],
                artifacts = [
                    new TreasureHunter()
                ]
            },
            ExeCardType = typeof(WethExe)
        });

        MoreDifficultiesApi?.RegisterAltStarters(WethDeck.Deck, new StarterDeck
        {
            cards = [
                new PulsedriveCard(),
                new Puckshot(),
            ],
            artifacts = 
            [
                new TreasureHunter()
            ]
        });

        /*
         * Statuses are used to achieve many mechanics.
         * However, statuses themselves do not contain any code - they just keep track of how much you have.
         */
        PulseStatus = helper.Content.Statuses.RegisterStatus("WethPulsedrive", new StatusConfiguration
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
        UnknownStatus = helper.Content.Statuses.RegisterStatus("UnknStatrus", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("4ab3ff"),
                icon = ModEntry.RegisterSprite(package, "assets/unknownlol.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "Unknown", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Unknown", "desc"]).Localize
        });

        JauntSlapSound = helper.Content.Audio.RegisterSound("spaceSlap", package.PackageRoot.GetRelativeFile("assets/SpaceSlap.wav"));
        //JauntSlapSound = RegisterSound(package, "assets/SpaceSlap.wav");
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
            Deck deck = WethDeck.Deck;
            if (DuoArtifactsApi is not null && WethDuoArtifacts.Contains(ta))
            {
                deck = DuoArtifactsApi.DuoArtifactVanillaDeck;
            }
            helper.Content.Artifacts.RegisterArtifact(ta.Name, UhDuhHundo.ArtifactRegistrationHelper(ta, RegisterSprite(package, "assets/Artifact/" + ta.Name + ".png").Sprite, deck));
        }
        SprArtTermMileCommon = RegisterSprite(package, "assets/Artifact/TerminusMilestonCommone.png").Sprite;
        SprArtTermMileBoss = RegisterSprite(package, "assets/Artifact/TerminusMilestoneBoss.png").Sprite;
        SprArtTermMileRelic = RegisterSprite(package, "assets/Artifact/TerminusMilestoneRelic.png").Sprite;
        SprArtTermJActive = RegisterSprite(package, "assets/Artifact/TerminusJauntActive.png").Sprite;
        SprArtTermJInactive = RegisterSprite(package, "assets/Artifact/TerminusJauntInactive.png").Sprite;
        SprArtTermJReward = RegisterSprite(package, "assets/Artifact/TerminusJauntReward.png").Sprite;
        SprArtTermJAltReward = RegisterSprite(package, "assets/Artifact/TerminusJauntAltReward.png").Sprite;
        SprArtTHDepleted = RegisterSprite(package, "assets/Artifact/TreasureHunterDepleted.png").Sprite;


        SprSplitshot = RegisterSprite(package, "assets/Splitshot.png").Sprite;
        SprSplitshotFail = RegisterSprite(package, "assets/SplitshotFail.png").Sprite;
        SprSplitshotPiercing = RegisterSprite(package, "assets/SplitshotPierce.png").Sprite;
        SprSplitshotPiercingFail = RegisterSprite(package, "assets/SplitshotPierceFail.png").Sprite;

        SprBayBlast = RegisterSprite(package, "assets/bayblast.png").Sprite;
        SprBayBlastFail = RegisterSprite(package, "assets/bayblastfail.png").Sprite;
        SprBayBlastWide = RegisterSprite(package, "assets/bayblastwide.png").Sprite;
        SprBayBlastWideFail = RegisterSprite(package, "assets/bayblastwidefail.png").Sprite;
        //DrawLoadingScreenFixer.Apply(Harmony);
        //SashaSportingSession.Apply(Harmony);

        SprGiantAsteroid = RegisterSprite(package, "assets/giantasteroid.png").Sprite;
        SprMegaAsteroid = RegisterSprite(package, "assets/megaasteroid.png").Sprite;
        SprGiantAsteroidIcon = RegisterSprite(package, "assets/giantasteroidicon.png").Sprite;
        SprMegaAsteroidIcon = RegisterSprite(package, "assets/megaasteroidicon.png").Sprite;

        /*
         * All the IRegisterable types placed into the static lists at the start of the class are initialized here.
         * This snippet invokes all of them, allowing them to register themselves with the package and helper.
         */
        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);

        Artifacthider.Apply(Harmony);
        SplitshotTranspiler.Apply(Harmony);
        ChoiceRelicRewardOfYourRelicChoice.Apply(Harmony);
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

    public static ISoundEntry RegisterSound(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Audio.RegisterSound(package.PackageRoot.GetRelativeFile(dir));
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

