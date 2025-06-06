using HarmonyLib;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using Weth.Artifacts;
using Weth.Cards;
using Weth.External;
using Weth.Conversation;

namespace Weth;

internal partial class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal static IPlayableCharacterEntryV2 WethTheSnep { get; private set; } = null!;
    internal string UniqueName { get; private set; }
    internal Harmony Harmony;
    internal IKokoroApi KokoroApi;
    internal IDeckEntry WethDeck;
    internal IDeckEntry GoodieDeck;
    public bool modDialogueInited;
    private int _loadFrameBuffer = 3;
    public bool WethFrameLoadAllowed
    {
        get => _loadFrameBuffer-- > 0;
    }
    internal IStatusEntry PulseStatus { get; private set; } = null!;
    internal IStatusEntry UnknownStatus { get; private set; } = null!;
    internal IModSoundEntry JauntSlapSound { get; private set; }
    internal IModSoundEntry SodaOpening { get; private set; }
    internal IModSoundEntry SodaOpened { get; private set; }
    internal IModSoundEntry HitHullHit { get; private set; }
    // internal IModSoundEntry MidiTestJourneyV { get; private set; }
    // internal IModSoundEntry MidiTestIncompetentB { get; private set; }
    public Spr PulseQuestionMark { get; private set; }
    // internal ICardTraitEntry AutoSU { get; private set; } = null!;
    // internal Spr AutoSUSpr { get; private set; }
    //internal ICardTraitEntry AutoE { get; private set; } = null!;

    public Spr WethEnd { get; private set; }
    public Spr WethEndrot { get; private set; }
    public Spr WethEndrotend { get; private set; }
    public Spr WethFramePast { get; private set; }
    public Spr WethFrameA { get; private set; }
    public Spr WethFrameB { get; private set; }
    public Spr WethFrameC { get; private set; }
    public Spr WethFrameOverlayA { get; private set; }
    public Spr WethFrameOverlayB { get; private set; }
    public Spr WethFrameOverlayC { get; private set; }
    public Spr WethFrameGlowA { get; private set; }
    public Spr WethFrameGlowB { get; private set; }
    public Spr WethFrameGlowC { get; private set; }
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

    public Spr SprArtMadcapDepleted { get; private set; }
    public Spr SprArtPowerSprintDepleted { get; private set; }
    public Spr SprArtBattleStimulationDepleted { get; private set; }
    public Spr SprArtBattleStimulationStimulated { get; private set; }
    public Spr SprArtCalculatedWhiffDepleted { get; private set; }
    public Spr SprArtCannonPrimerDepleted { get; private set; }

    public Spr SprSplitshot { get; private set; }
    public Spr SprSplitshotFail { get; private set; }
    public Spr SprSplitshotPiercing { get; private set; }
    public Spr SprSplitshotPiercingFail { get; private set; }

    public Spr SprBayBlast { get; private set; }
    public Spr SprBayBlastFail { get; private set; }
    public Spr SprBayBlastWide { get; private set; }
    public Spr SprBayBlastWideFail { get; private set; }
    public Spr SprBayBlastFlared { get; private set; }
    public Spr SprBayBlastFlaredFail { get; private set; }
    public Spr SprBayBlastGeneralFail { get; private set; }

    public Spr SprGiantAsteroidIcon { get; private set; }
    public Spr SprGiantAsteroid { get; private set; }
    public Spr SprMegaAsteroidIcon { get; private set; }
    public Spr SprMegaAsteroid { get; private set; }

    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; private set; } = null!;
    internal IDuoArtifactsApi? DuoArtifactsApi { get; private set; } = null!;
    public LocalDB localDB { get; set; } = null!;
    
    /*
     * The following lists contain references to all types that will be registered to the game.
     * All cards and artifacts must be registered before they may be used in the game.
     * In theory only one collection could be used, containing all registerable types, but it is seperated this way for ease of organization.
     */
    private readonly static List<Type> WethCommonCardTypes = [
        typeof(WethExe),
        typeof(TripleTap),
        typeof(Puckshot),
        typeof(SplitshotCard),
        typeof(TrashDispenser),
        typeof(CargoBlaster),
        typeof(PulsedriveCard),
        typeof(GiantTrash),
        typeof(DoubleBlast),
        typeof(Overcompensator),
        typeof(MilkSoda),
        typeof(Feral)
    ];
    private readonly static List<Type> WethUncommonCardTypes = [
        typeof(DoubleTap),
        typeof(Disabler),
        typeof(ScatterTrash),
        typeof(Discovery),
        typeof(Powershot),
        typeof(Spreadshot),
        typeof(Bloom),
        typeof(FeralBlast),
        typeof(MirageBlast)
    ];
    private readonly static List<Type> WethRareCardTypes = [
        typeof(UnstoppableForce),
        typeof(PearlDispenser),
        typeof(CrisisCall),
        typeof(PowPow),
        typeof(ExtremeViolence)
    ];
    private readonly static List<Type> WethSpecialCardTypes = [
        typeof(CryAhtack),
        typeof(CryDuhfend),
        typeof(CryCapacity),
        typeof(CryEnergy),
        typeof(CryEvade),
        typeof(CryFlux),
        typeof(CryShield),
        typeof(CrySwap),
        typeof(MechAhtack),
        typeof(MechDuhfend),
        typeof(MechEvade),
        typeof(MechHull),
        typeof(MechBubble),
        typeof(MechMissile),
        typeof(MechDodge),
        typeof(MechSwap),
        typeof(CryPlaceholder),
        typeof(MechPlaceholder),
        typeof(FullCommitment)
        // typeof(PlayJourneyV),
        // typeof(PlayIncompetentBaffoon)
    ];
    private readonly static IEnumerable<Type> WethCardTypes =
        WethCommonCardTypes
            .Concat(WethUncommonCardTypes)
            .Concat(WethRareCardTypes)
            .Concat(WethSpecialCardTypes);

    private readonly static List<Type> WethCommonArtifacts = [
        typeof(TreasureSeeker),
        typeof(HiddenOptions),
        typeof(BattleStimulation),
        typeof(CalculatedWhiff),
        typeof(CannonPrimer)
    ];
    private readonly static List<Type> WethBossArtifacts = [
        typeof(HiddenOptions2),
        typeof(TerminusMilestone),
        typeof(TerminusJaunt),
        typeof(AlPoToCa),
        typeof(SuperDriveCollector)
    ];
    private readonly static List<Type> WethEventArtifacts = [
        typeof(TreasureHunter),
        typeof(SpaceRelics),
        typeof(SpaceRelics2),
        typeof(SR2Crackling),
        typeof(SR2Focused),
        typeof(SR2Subsuming)
    ];
    private readonly static List<Type> WethSpecialArtifacts = [
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
    private readonly static List<Type> WethDuoArtifacts = [
        typeof(CannonRecharge),  // CAT
        typeof(ResidualShot),  // Peri
        typeof(RockPower),  // Isaac
        typeof(PowerCrystals),  // Books
        typeof(PyroCannon),  // Drake
        typeof(MadcapCharge),  // Dizzy
        typeof(PowerSprint),  // Riggs
        typeof(HiddenGem),  // Max
    ];
    private readonly static IEnumerable<Type> WethArtifactTypes =
        WethCommonArtifacts
            .Concat(WethBossArtifacts)
            .Concat(WethEventArtifacts)
            .Concat(WethSpecialArtifacts)
            .Concat(WethDuoArtifacts);

    private readonly static List<Type> WethDialogues = [
        typeof(StoryDialogue),
        typeof(EventDialogue),
        typeof(CombatDialogue),
        typeof(ArtifactDialogue),
        typeof(CardDialogue),
        typeof(MemoryDialogue)
    ];
    private readonly static IEnumerable<Type> AllRegisterableTypes =
        WethCardTypes
            .Concat(WethDialogues);

    private static List<string> Weth1Anims = [
        "crystallized",
        "feraldie",
        "feralkill",
        "gameover",
        "mini",
        "placeholder",
        "sodadrink",
        "sodagone",
        "sodaexplode",
        "sodaexplodeup",
        "sodaexplodedown",
        "sodashakedown",
        "sodashakeup",
        "traumatised",
        "pastwait",
        "pastcheese",
        "pastglare",
        "pastnotpresent",
        "pastglareoffscreenextinguisher",
        "pastglarewithextinguisher",
        "pastglareoffscreen",
        "outsidetest",
        "squintoffscreen"
    ];
    private static List<string> Weth3Anims = [
        "angry",
        "angrypointing",
        "bringiton",
        "cryingcat",
        "crystal",
        "crystallolipop",
        "down",
        "evil",
        "facepalm",
        "hmm",
        "lockedin",
        "mad",
        "touch",
        "yay",
        "up",
        "pastscream",
        "pastfacepalm",
        "pastlockedin",
        "pastmad",
        "pastsilly",
        "pastlookfor",
        "pastexhausted",
    ];
    private static List<string> Weth4Anims = [
        "pain",
        "pastsurprise",
        "sob",
        "surprise",
        "terror",
    ];
    private static List<string> Weth5Anims = [
        "apple",
        "complain",
        "contemplate",
        "donewithit",
        "dontcare",
        "explain",
        "happy",
        "happynothoughts",
        "intense",
        "isthisa",
        "lookatthat",
        "lookatyou",
        "neutral",
        "panic",
        "plead",
        "pointing",
        "pointout",
        "sad",
        "shrug",
        "sly",
        "sparkle",
        "squint",
        "think",
        "tired",
        "verysad",
        "verysilly",
        "wtf",
        "whatsthis",
        "pastneutral",
        "pastsquint",
        "pastplead",
        "pastexplain",
        "pasteyeroll",
        "pastsparkle",
        "pasttired",
        "pasthappy",
        "pastdonewithit",
    ];
    private static List<string> Weth6Anims = [
        "pastputoutfire",
        //"maniac",  (ace attorney big evil dude from game 1, including the clapping)
    ];
    public readonly static IEnumerable<string> WethAnims =
        Weth1Anims
            .Concat(Weth3Anims)
            .Concat(Weth4Anims)
            .Concat(Weth5Anims)
            .Concat(Weth6Anims);

    public static bool Patch_EnemyPack {get; private set;}
}