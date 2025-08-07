using System;
using System.Collections.Generic;
using System.Reflection;
using daisyowl.text;
using FMOD;
using FSPRO;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;
using Weth.External;

namespace Weth.Cards;


/// <summary>
/// Shake to gamble! TODO: Make card shake when in hand and recently shook
/// </summary>
public class NewMilkSoda : Card, IRegisterable
{
    public bool Exploded { get; set; } = false;
    public bool Used { get; set; } = false;
    public bool Demo { get; set; } = true;
    public bool Unavailable => Exploded || Used;
    public int Shakes { get; set; } = 0;
    public bool Special { get; set; } = false;
    public static Spr MilkSoda_BG { get; set; }  // BackGround
    public static Spr MilkSoda_BE { get; set; }  // BackGround (exploded)
    public static Spr MilkSoda_OL { get; set; }  // OverLay
    public static Spr MilkSoda_SH { get; set; }  // SHine
    public static Spr MilkSoda_TX { get; set; }  // TeXt
    public double ExplosionChance { get; set; } = 0;
    //public int PulsedrivesAcquired { get; set; } = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        MilkSoda_BG = ModEntry.RegisterSprite(package, "assets/Card/0/milksoda_bg.png").Sprite;
        MilkSoda_BE = ModEntry.RegisterSprite(package, "assets/Card/0/milksoda_boom.png").Sprite;
        MilkSoda_OL = ModEntry.RegisterSprite(package, "assets/Card/0/milksoda_overlay.png").Sprite;
        MilkSoda_SH = ModEntry.RegisterSprite(package, "assets/Card/0/milksoda_shine.png").Sprite;
        MilkSoda_TX = ModEntry.RegisterSprite(package, "assets/Card/0/milksoda_text.png").Sprite;
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B],
                unreleased = true,
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "MilkSoda", "name"]).Localize,
            Art = MilkSoda_BG
            //Art = ModEntry.RegisterSprite(package, "assets/Card/2/disabler.png").Sprite
        });
        ModEntry.Instance.KokoroApi.V2.CardRendering.RegisterHook(new MilkSodayHook());
        Color c = Color.Lerp(new Color("93c4c8"), Colors.white, 0.6);
    }


    public static int GetShakenAmount(Upgrade u)
    {
        return u switch
        {
            // Upgrade.B => 4,
            // Upgrade.A => 3,
            _ => 2
        };
    }

    public static int GetShakenAmount(bool special)
    {
        return special ? 1 : 2;
    }

    public double GetMultiplier()
    {
        if (upgrade == Upgrade.A)
        {
            return 0.1;
        }
        else if (upgrade == Upgrade.B)
        {
            return 0.15;
        }
        else if ((int)upgrade == 3)
        {
            return Math.Pow(0.5, Shakes + 1);
        }
        return 0.2;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return Unavailable ? [new ASodaDescription{description = ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "empty"])}] :
        [
            new ADummyAction(),
            new ADummyAction(),
            ModEntry.Instance.KokoroApi.V2.SpoofedActions.MakeAction(
                new APseudoPulsedriveGiver
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = Shakes,
                    targetPlayer = true,
                    descriptions = [
                        ("milksodaMain", GetTooltip()),
                        ("milksodaWarning", ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "flip"], new {
                            shaker = GetShakenAmount(Special)
                        })),
                        //("milksodaShakes", string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descc"]), Shakes)),
                        //("milksodaprobabilities", GetEvenMoreTooltips()),
                        //("milksodaprobabilities2", string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descp"]), GetMultiplier())),

                    ]
                },
                new ADummyAction()
            ).AsCardAction,
            new ADummyAction(),
            ModEntry.Instance.KokoroApi.V2.SpoofedActions.MakeAction(
                new AWethPercentageRenderer
                {
                    status = Status.drawLessNextTurn,
                    statusAmount = (int)(ExplosionChance * 100),
                    targetPlayer = true
                },
                new ADummyAction()
            ).AsCardAction,
        ];
    }


    public override void AfterWasPlayed(State state, Combat c)
    {
        if (!Unavailable)
        {
            if(GonnaExplode(state.rngActions))
            {

                DoExplosion(c, true, false);
                c.QueueImmediate([
                    new ASodaOpen
                    {
                        dialogueSelector = flipped? ".shakeSodaUp" : ".shakeSodaDown",
                        sound = ModEntry.Instance.SodaOpening,
                        vol = 0.5f,
                        timer = 0.4
                    },
                    new ASodaBoom
                    {
                        dialogueSelector = flipped? ".shakeSodaBoomUp" : ".shakeSodaBoomDown",
                        shakenAmount = GetShakenAmount(Special)
                    }
                ]);
            }
            else if (Shakes > 0)
            {
                c.QueueImmediate([
                    new ASodaOpen
                    {
                        dialogueSelector = flipped? ".shakeSodaUp" : ".shakeSodaDown",
                        sound = ModEntry.Instance.SodaOpening,
                        vol = 0.5f,
                        timer = 0.5
                    },
                    new ASodaOpen
                    {
                        dialogueSelector = ".shakeSodaDrink",
                        sound = ModEntry.Instance.SodaOpened,
                        vol = 0.5f,
                        timer = 0.0
                    },
                    new AStatus
                    {
                        status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                        statusAmount = Shakes,
                        targetPlayer = true,
                    }
                ]);
                Used = true;
            }
        }
    }

    public override void OnFlip(G g)
    {
        if (!Unavailable){
            ExplosionChance += GetMultiplier();
            Shakes++;
            if (ExplosionChance >= 1 && g.state.route is Combat c)
            {
                DoExplosion(c);
                return;
            }
            else
            {
                Audio.Play(new GUID?(Event.Status_EvadeUp), true);
            }
            if (g.state.route is Combat c0)
            {
                c0.QueueImmediate(new ADummyAction
                {
                    dialogueSelector = flipped? ".shakeSodaUp" : ".shakeSodaDown"
                });
            }
        }
    }


    public override void OnExitCombat(State s, Combat c)
    {
        Shakes = 0;
        ExplosionChance = 0.0;
        Exploded = Used = false;
        Demo = true;
    }

    public override void OnDraw(State s, Combat c)
    {
        Demo = false;
    }


    private void DoExplosion(Combat combat, bool refund = false, bool explode = true)
    {
        flipped = false;
        if (refund)
        {
            combat.QueueImmediate(new AEnergy
            {
                changeAmount = GetCost(),
            });
        }
        if (explode)
        {
            combat.QueueImmediate(new ASodaBoom
            {
                dialogueSelector = ".shakeSodaGone",
                shakenAmount = GetShakenAmount(Special)
            });
        }
        Exploded = true;
    }

    private bool GonnaExplode(Rand rng)
    {
        return Mutil.Roll(rng.Next(), (ExplosionChance, true), (1 - ExplosionChance, false));
    }

    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = GetCost(),
                artTint = "93c4c8",
                retain = upgrade == Upgrade.B,
                exhaust = true,
                floppable = !Exploded,
                artOverlay = MilkSoda_OL,
                description = GetDescription(),
                art = GetSprite(),
                // description = string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", GetDescription()]), PulsedrivesAcquired)
            }
        };
    }

    public int GetCost()
    {
        if (Unavailable) return 0;
        if (upgrade == Upgrade.B) return 2;
        return 1;
    }

    /// <summary>
    /// Get description for when the card is not active or has been exploded.
    /// </summary>
    /// <returns></returns>
    private string? GetDescription()
    {
        if (Used)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "spent"]);  // Used description
        }
        if (Exploded)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "exploded"], new
            {
                shaker = GetShakenAmount(Special)
            });  // Exploded description
        }
        if (Demo)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", ((int)upgrade == 3 ? "demoC" : "demo")], new
            {
                rate = (int)(GetMultiplier() * 100),
                shaker = GetShakenAmount(Special)
            });  // Exploded description
        }
        // if (flipped)
        // {
        //     return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "flip"], new
        //     {
        //         shaker = GetShakenAmount(upgrade)
        //     });
        // }
        // return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", "desc"], new
        // {
        //     amount = Shakes,
        //     explode = (int)(ExplosionChance * 100)
        // });
        return null;
    }

    private Spr? GetSprite()
    {
        if (Used)
        {
            return StableSpr.cards_Repairs;  // TODO: Have a healing thing
        }
        if (Exploded)
        {
            return MilkSoda_BE;
        }
        return MilkSoda_BG;
    }

    public override void ExtraRender(G g, Vec v)
    {
        Color col = Color.Lerp(new Color("2a767d"), Colors.white, 0.6);
        if (g?.state?.route is Combat c && c.energy < GetCost())
        {
            col = Color.Lerp(Colors.textMain.fadeAlpha(0.55), Colors.redd, shakeNoAnim);
        }
        Draw.Sprite(MilkSoda_TX, v.x - 3, v.y - 4, color:col);
        Draw.Sprite(MilkSoda_SH, v.x - 2, v.y - 2, color:new Color("2a767d"), blend: BlendMode.Screen);
    }

    /// <summary>
    /// Gets the tooltip for the card that basically describes how to use the card and the risk it comes with.
    /// </summary>
    /// <returns></returns>
    private string GetTooltip()
    {
        return ModEntry.Instance.Localizations.Localize(["card", "Token", "MilkSoda", (int)(upgrade) == 3 ? "tooltipC" : "tooltip"], new
        {
            amount = Shakes,
            rate = (int)(GetMultiplier() * 100)
        });
    }

    private sealed class MilkSodayHook : IKokoroApi.IV2.ICardRenderingApi.IHook
    {
        public Font? ReplaceTextCardFont(IKokoroApi.IV2.ICardRenderingApi.IHook.IReplaceTextCardFontArgs args)
        {
            if (args.Card is NewMilkSoda ms && ms.Demo)
            {
                return ModEntry.Instance.KokoroApi.V2.Assets.PinchCompactFont;
            }
            return null;
        }
    }

}