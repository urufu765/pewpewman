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
using Weth.Conversation;
//using System.Reflection;

namespace Weth;

internal partial class ModEntry : SimpleMod
{
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
                        DuoArtifactMeta? dam = type.GetCustomAttribute<DuoArtifactMeta>();
                        if (dam is not null)
                        {
                            if (dam.duoModDeck is null)
                            {
                                DuoArtifactsApi.RegisterDuoArtifact(type, [WethDeck!.Deck, dam.duoDeck]);
                            }
                            else if (helper.Content.Decks.LookupByUniqueName(dam.duoModDeck) is IDeckEntry ide)
                            {
                                DuoArtifactsApi.RegisterDuoArtifact(type, [WethDeck!.Deck, ide.Deck]);
                            }
                        }
                    }
                }
                Patch_EnemyPack = helper.ModRegistry.LoadedMods.ContainsKey("TheJazMaster.EnemyPack");
                foreach (Type type in WethDialogues)
                    AccessTools.DeclaredMethod(type, nameof(IDialogueRegisterable.LateRegister))?.Invoke(null, null);
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
            BorderSprite = RegisterSprite(package, "assets/Borders/frame_weth.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Weth", "name"]).Localize,
            ShineColorOverride = _ => new Color(0, 0, 0),
        });
        WethCommon = RegisterSprite(package, "assets/Borders/frame_wethcommon.png").Sprite;
        WethUncommon = RegisterSprite(package, "assets/Borders/frame_wethuncommon.png").Sprite;
        WethRare = RegisterSprite(package, "assets/Borders/frame_wethrare.png").Sprite;
        Vault.charsWithLore.Add(WethDeck.Deck);
        WethEnd = RegisterSprite(package, "assets/Memry/weth_end.png").Sprite;
        BGRunWin.charFullBodySprites.Add(WethDeck.Deck, WethEnd);
        WethEndrot = RegisterSprite(package, "assets/Memry/weth1_end.png").Sprite;
        WethEndrotend = RegisterSprite(package, "assets/Memry/weth2_end.png").Sprite;
        DB.backgrounds.Add("BGWethCustomRings", typeof(BGWethRings));
        DB.backgrounds.Add("BGWethShop", typeof(BGWethShop));
        DB.backgrounds.Add("BGWethVault", typeof(BGWethVault));
        WethFramePast = RegisterSprite(package, "assets/Frames/char_frame_weth.png").Sprite;
        WethFrameA = RegisterSprite(package, "assets/Frames/char_frame_wetha.png").Sprite;
        WethFrameB = RegisterSprite(package, "assets/Frames/char_frame_wethb.png").Sprite;
        WethFrameC = RegisterSprite(package, "assets/Frames/char_frame_wethc.png").Sprite;
        WethFrameOverlayA = RegisterSprite(package, "assets/Frames/char_frame_overlay_wetha.png").Sprite;
        WethFrameOverlayB = RegisterSprite(package, "assets/Frames/char_frame_overlay_wethb.png").Sprite;
        WethFrameOverlayC = RegisterSprite(package, "assets/Frames/char_frame_overlay_wethc.png").Sprite;
        WethFrameGlowA = RegisterSprite(package, "assets/Frames/char_frame_glow_wetha.png").Sprite;
        WethFrameGlowB = RegisterSprite(package, "assets/Frames/char_frame_glow_wethb.png").Sprite;
        WethFrameGlowC = RegisterSprite(package, "assets/Frames/char_frame_glow_wethc.png").Sprite;

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
            BorderSprite = RegisterSprite(package, "assets/Borders/frame_goodies.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Goodie", "name"]).Localize,
            //ShineColorOverride = _ => new Color(0, 0, 0),
        });
        GoodieCrystal = RegisterSprite(package, "assets/Borders/frame_goodiescrystal.png").Sprite;
        GoodieCrystalA = RegisterSprite(package, "assets/Borders/frame_goodiescrystalA.png").Sprite;
        GoodieMech = RegisterSprite(package, "assets/Borders/frame_goodiesmech.png").Sprite;
        GoodieMechA = RegisterSprite(package, "assets/Borders/frame_goodiesmechA.png").Sprite;


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
            BorderSprite = WethFramePast,
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
                icon = ModEntry.RegisterSprite(package, "assets/Icon/pulsedrive.png").Sprite
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
                icon = ModEntry.RegisterSprite(package, "assets/Icon/unknownlol.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "Unknown", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Unknown", "desc"]).Localize
        });
        JauntSlapSound = helper.Content.Audio.RegisterSound("spaceSlap", package.PackageRoot.GetRelativeFile("assets/SFX/SpaceSlap.ogg"));
        SodaOpening = helper.Content.Audio.RegisterSound("sodaopening", package.PackageRoot.GetRelativeFile("assets/SFX/sodaopening.wav"));
        SodaOpened = helper.Content.Audio.RegisterSound("sodaopened", package.PackageRoot.GetRelativeFile("assets/SFX/sodaopen.wav"));
        HitHullHit = helper.Content.Audio.RegisterSound("vanillahullhitbutvariablepitch", package.PackageRoot.GetRelativeFile("assets/SFX/HitsHurtSeparated.ogg"));
        MidiTestJourneyV = helper.Content.Audio.RegisterSound("MidiTestJourney", package.PackageRoot.GetRelativeFile("assets/SFX/Journey_Track_V.mid"));
        MidiTestIncompetentB = helper.Content.Audio.RegisterSound("MidiTestIncompetent", package.PackageRoot.GetRelativeFile("assets/SFX/Incompetent_Baffoon.mid"));
        PulseQuestionMark = RegisterSprite(package, "assets/Icon/questionmark.png").Sprite;

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
        //_ = new Otherdriving();

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
            if (WethDuoArtifacts.Contains(ta))
            {
                if (DuoArtifactsApi is null)
                {
                    continue;
                }
                else
                {
                    deck = DuoArtifactsApi.DuoArtifactVanillaDeck;
                }
            }
            helper.Content.Artifacts.RegisterArtifact(ta.Name, UhDuhHundo.ArtifactRegistrationHelper(ta, RegisterSprite(package, "assets/Artifact/" + ta.Name + ".png").Sprite, deck));
        }
        SprArtTermMileCommon = RegisterSprite(package, "assets/Artifact/TerminusMilestoneCommon.png").Sprite;
        SprArtTermMileBoss = RegisterSprite(package, "assets/Artifact/TerminusMilestoneBoss.png").Sprite;
        SprArtTermMileRelic = RegisterSprite(package, "assets/Artifact/TerminusMilestoneRelic.png").Sprite;
        SprArtTermJActive = RegisterSprite(package, "assets/Artifact/TerminusJauntActive.png").Sprite;
        SprArtTermJInactive = RegisterSprite(package, "assets/Artifact/TerminusJauntInactive.png").Sprite;
        SprArtTermJReward = RegisterSprite(package, "assets/Artifact/TerminusJauntReward.png").Sprite;
        SprArtTermJAltReward = RegisterSprite(package, "assets/Artifact/TerminusJauntAltReward.png").Sprite;
        SprArtTHDepleted = RegisterSprite(package, "assets/Artifact/TreasureHunterDepleted.png").Sprite;
        SprArtMadcapDepleted = RegisterSprite(package, "assets/Artifact/MadcapDepleted.png").Sprite;
        SprArtPowerSprintDepleted = RegisterSprite(package, "assets/Artifact/PowerSprintDepleted.png").Sprite;


        SprSplitshot = RegisterSprite(package, "assets/Icon/Splitshot.png").Sprite;
        SprSplitshotFail = RegisterSprite(package, "assets/Icon/SplitshotFail.png").Sprite;
        SprSplitshotPiercing = RegisterSprite(package, "assets/Icon/SplitshotPierce.png").Sprite;
        SprSplitshotPiercingFail = RegisterSprite(package, "assets/SplitshotPierceFail.png").Sprite;

        SprBayBlast = RegisterSprite(package, "assets/Icon/bayblast.png").Sprite;
        SprBayBlastFail = RegisterSprite(package, "assets/Icon/bayblastfail.png").Sprite;
        SprBayBlastWide = RegisterSprite(package, "assets/Icon/bayblastwide.png").Sprite;
        SprBayBlastWideFail = RegisterSprite(package, "assets/Icon/bayblastwidefail.png").Sprite;
        SprBayBlastFlared = RegisterSprite(package, "assets/Icon/bayblastflared.png").Sprite;
        SprBayBlastFlaredFail = RegisterSprite(package, "assets/Icon/bayblastflaredfail.png").Sprite;
        SprBayBlastGeneralFail = RegisterSprite(package, "assets/Icon/bayblastgeneralfail.png").Sprite;
        //DrawLoadingScreenFixer.Apply(Harmony);
        //SashaSportingSession.Apply(Harmony);

        SprGiantAsteroid = RegisterSprite(package, "assets/Drone/giantasteroid.png").Sprite;
        SprMegaAsteroid = RegisterSprite(package, "assets/Drone/megaasteroid.png").Sprite;
        SprGiantAsteroidIcon = RegisterSprite(package, "assets/Icon/giantasteroidicon.png").Sprite;
        SprMegaAsteroidIcon = RegisterSprite(package, "assets/Icon/megaasteroidicon.png").Sprite;

        /*
         * All the IRegisterable types placed into the static lists at the start of the class are initialized here.
         * This snippet invokes all of them, allowing them to register themselves with the package and helper.
         */
        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);

        Apply(Harmony);
        //BayBlastIconography.Apply(Harmony);
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

