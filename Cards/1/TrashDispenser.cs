using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shoots a trash
/// </summary>
public class TrashDispenser : Card, IRegisterable
{
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "TrashDispenser", "name"]).Localize,
            Art = StableSpr.cards_SmallBoulder
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    }
                },
                new AStatus
                {
                    status = Status.droneShift,
                    statusAmount = 2,
                    targetPlayer = true
                }            
            ],
            _ => 
            [
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    }
                },
                new AStatus
                {
                    status = Status.droneShift,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.A => new CardData
            {
                cost = 0,
                artTint = "696969",
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 1,
                artTint = "696969",
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}