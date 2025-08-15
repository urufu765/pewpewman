using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Heal?
/// </summary>
public class PearlDispenser : WCRare, IRegisterable
{
    // private static Spr altSprite {get; set;}
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Rare", "PearlDispenser", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/3/pearldispenser.png").Sprite
        });
        // altSprite = ModEntry.RegisterSprite(package, "assets/Card/3/pearldispenseralt.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ASpawn
                {
                    thing = new RepairKit
                    {
                        yAnimation = 0.0
                    },
                    offset = -1
                },
                new ASpawn
                {
                    thing = new RepairKit
                    {
                        yAnimation = 0.0
                    },
                    offset = 1
                },
            ],
            _ => 
            [
                new ASpawn
                {
                    thing = new RepairKit
                    {
                        yAnimation = 0.0,
                        bubbleShield = true
                    }
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
                cost = 3,
                artOverlay = ModEntry.Instance.WethRare,
                art = StableSpr.cards_Repairs,
            },            
            Upgrade.A => new CardData
            {
                cost = 2,
                artOverlay = ModEntry.Instance.WethRare,
            },
            _ => new CardData
            {
                cost = 2,
                artOverlay = ModEntry.Instance.WethRare,
                exhaust = true
            }
        };
    }
}