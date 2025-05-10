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
/// Shake to gamble!
/// </summary>
public class MilkSoda : Card, IRegisterable
{
    public bool Exploded { get; set; } = false;
    public bool Used { get; set; } = false;
    public bool Unavailable => Exploded || Used;
    public int NextGoal { get; set; } = 0;
    public int Shakes { get; set; } = 0;
    public int PulsedrivesAcquired { get; set; } = 0;
    //public static List<(double, int)> GeneratedProbabilityCurve {get; set;} = null!;
    public static Dictionary<Upgrade, (double, int)[]> PregenMilkOddsTable {get; set;} = new();
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "MilkSoda", "name"]).Localize,
            Art = StableSpr.cards_colorless
            //Art = ModEntry.RegisterSprite(package, "assets/Card/2/disabler.png").Sprite
        });
        PregenMilkOddsTable[Upgrade.None] = [.. BellCurveIt(10, 1.5, -0.25)];
        PregenMilkOddsTable[Upgrade.A] = [.. BellCurveIt(15, 3, -0.2)];
        PregenMilkOddsTable[Upgrade.B] = [(1.0, 1)];
        //ModEntry.Instance.KokoroApi.V2.CardRendering.RegisterHook(new MilkSodayHook());
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return Unavailable? [] : 
        [
            ModEntry.Instance.KokoroApi.V2.SpoofedActions.MakeAction(
                new APseudoPulsedriveGiver
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = PulsedrivesAcquired,
                    targetPlayer = true,
                    descriptions = [
                        ("milksodaMain", GetTooltip()),
                        ("milksodaWarning", ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descf"])),
                        //("milksodaShakes", string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descc"]), Shakes)),
                        //("milksodaprobabilities", GetEvenMoreTooltips()),
                        //("milksodaprobabilities2", string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descp"]), GetMultiplier())),

                    ]
                },
                new ADummyAction()
            ).AsCardAction
        ];
    }


    public override void AfterWasPlayed(State state, Combat c)
    {
        if (!Unavailable)
        {
            if(GonnaExplode(state.rngActions))
            {

                DoExplosion(c, true);
                c.QueueImmediate([
                    new ASodaOpen
                    {
                        dialogueSelector = flipped? ".shakeSodaUp" : ".shakeSodaDown",
                        sound = ModEntry.Instance.SodaOpening,
                        vol = 0.5f,
                        timer = 0.4
                    },
                    new ADummyAction
                    {
                        dialogueSelector = flipped? ".shakeSodaBoomUp" : ".shakeSodaBoomDown"
                    }
                ]);
            }
            else if (PulsedrivesAcquired > 0)
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
                        statusAmount = PulsedrivesAcquired,
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
            Shakes++;
            if (Shakes * GetMultiplier() >= 100 && g.state.route is Combat c)
            {
                DoExplosion(c);
                c.QueueImmediate(new ADummyAction
                {
                    dialogueSelector = ".shakeSodaGone"
                });
                return;
            }
            else if (Shakes == NextGoal)
            {
                PulsedrivesAcquired++;
                NextGoal += GetNextGoal(g.state.rngActions, upgrade);
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

    public override void OnDraw(State s, Combat c)
    {
        if (NextGoal == 0)
        {
            NextGoal += GetNextGoal(s.rngActions, upgrade);
        }
    }

    public override void OnExitCombat(State s, Combat c)
    {
        Shakes = PulsedrivesAcquired = NextGoal = 0;
        Exploded = Used = false;
    }
    

    private void DoExplosion(Combat combat, bool refund = false)
    {
        Exploded = true;
        NextGoal = 0;
        flipped = false;
        if (refund)
        {
            combat.QueueImmediate(new AEnergy
            {
                changeAmount = 1,
            });
        }
        combat.QueueImmediate(new AHurt
        {
            hurtAmount = 2,
            targetPlayer = true
        });
    }


    private static int GetNextGoal(Rand rng, Upgrade upgrade = Upgrade.None)
    {
        return Mutil.Roll(
            rng.Next(), (0.00, 0), PregenMilkOddsTable[upgrade]
        );
    }

    /// <summary>
    /// A skewed standard 
    /// </summary>
    /// <param name="size"></param>
    /// <param name="stdDev"></param>
    /// <param name="skew"></param>
    /// <returns></returns>
    private static List<(double, int)> BellCurveIt(int size, double stdDev = 1.0, double skew = 0.0)
    {
        double total = 0.0;
        double mean = size / 2.0;
        List<(double, int)> result = [];
        List<double> probabilities = [];
        for (int a = 1; a <= size; a++)
        {
            double probability = Math.Exp(-0.5 * Math.Pow(
                    (a - mean) / stdDev, 2
                    ))
                *
                Math.Max(0, 1 + (skew * ((a - mean) / stdDev))) / 
                (stdDev * Math.Sqrt(2 * Math.PI));
            total += probability;
            probabilities.Add(probability);
        }

        for (int b = 0; b < size; b++)
        {
            result.Add((probabilities[b] / total, b + 1));
        }
        return result;
    }

    private bool GonnaExplode(Rand rng)
    {
        double chance = Shakes * GetMultiplier() / 100.0;
        return Mutil.Roll(rng.Next(), (chance, true), (1 - chance, false));
    }

    private double GetMultiplier()
    {
        return upgrade switch
        {
            Upgrade.A => 1,
            Upgrade.B => 10,
            _ => 2.5
        };
    }

    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = Exploded? 0 : 1,
                artTint = "ffffff",
                exhaust = true,
                floppable = !Exploded,
                artOverlay = ModEntry.Instance.WethCommon,
                description = GetDescription(),
                art = GetSprite(),
                // description = string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", GetDescription()]), PulsedrivesAcquired)
            }
        };
    }

    /// <summary>
    /// Get description for when the card is not active or has been exploded.
    /// </summary>
    /// <returns></returns>
    private string? GetDescription()
    {
        if (NextGoal == 0 && flipped)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descf"]);  // Warning description
        }
        if (Used)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "descu"]);  // Used description
        }
        if (Exploded)
        {
            return ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", "desce"]);  // Exploded description
        }
        return null;
    }

    private Spr? GetSprite()
    {
        if (Used)
        {
            return null;
        }
        if (Exploded)
        {
            return null;
        }
        return null;
    }

    /// <summary>
    /// Gets the tooltip for the card that basically describes how to use the card and the risk it comes with.
    /// </summary>
    /// <returns></returns>
    private string GetTooltip()
    {
        return string.Format(ModEntry.Instance.Localizations.Localize(["card", "Common", "MilkSoda", upgrade switch {
            Upgrade.A => "desca",
            Upgrade.B => "descb",
            _ => "desc"
        }]), PulsedrivesAcquired, Shakes * GetMultiplier() / 100.0);
    }

    /// <summary>
    /// Gets all the odds for next goal distance
    /// </summary>
    /// <returns></returns>
    private string GetEvenMoreTooltips()
    {
        string result = "";
        foreach ((double prob, int goal) in PregenMilkOddsTable[upgrade])
        {
            result += string.Format("{0}: {1:p}\n", goal, prob);
        }
        return result;
    }

    // private sealed class MilkSodayHook : IKokoroApi.IV2.ICardRenderingApi.IHook
    // {
    //     public Font? ReplaceTextCardFont(IKokoroApi.IV2.ICardRenderingApi.IHook.IReplaceTextCardFontArgs args)
    //     {
    //         if (args.Card is MilkSoda ms && !ms.Exploded)
    //         {
    //             return ModEntry.Instance.KokoroApi.V2.Assets.PinchCompactFont;
    //         }
    //         return null;
    //     }
    // }

}