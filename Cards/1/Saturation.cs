using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class Feral : Card, IRegisterable, IHasCustomCardTraits
{
    private static Spr closedSprite;
    private static Spr openSprite;
    public bool FlagState {get; set;} = false;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "Feral", "name"]).Localize
        });
        openSprite = ModEntry.RegisterSprite(package, "assets/Card/1/feralalt.png").Sprite;
        closedSprite = ModEntry.RegisterSprite(package, "assets/Card/1/feral.png").Sprite;
    }


    // public override void AfterWasPlayed(State state, Combat c)
    // {
    //     FlagState = !FlagState;
    //     // ModEntry.Instance.Logger.LogInformation("Play?" + FlagState);
    // }

    public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition)
    {
        FlagState = !FlagState;
        // ModEntry.Instance.Logger.LogInformation("Other?" + FlagState);
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                // new AAttack
                // {
                //     damage = GetDmg(s, 1),
                //     fast = true
                // },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    stunEnemy = true,
                    fast = true
                },
            ],
            Upgrade.A => 
            [
                // new AAttack
                // {
                //     damage = GetDmg(s, 1),
                //     piercing = true,
                //     fast = true
                // },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true,
                    fast = true
                },            
            ],
            _ => 
            [
                // new AAttack
                // {
                //     damage = GetDmg(s, 1),
                //     fast = true
                // },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                },
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 1,
                infinite = true,
                artTint = "b38144",
                art = FlagState ? openSprite : closedSprite,
                artOverlay = ModEntry.Instance.WethCommon
            },
            Upgrade.A => new CardData
            {
                cost = 1,
                infinite = true,
                artTint = "b34444",
                art = FlagState ? openSprite : closedSprite,
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 1,
                infinite = true,
                art = FlagState ? openSprite : closedSprite,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }

    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        return new HashSet<ICardTraitEntry>{ModEntry.Instance.KokoroApi.V2.Fleeting.Trait};
    }
}