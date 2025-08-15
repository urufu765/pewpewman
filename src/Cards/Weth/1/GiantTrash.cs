using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Objects;

namespace Weth.Cards;

/// <summary>
/// Shoots a trash
/// </summary>
public class GiantTrash : WCCommon, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Common", "GiantTrash", "name"]).Localize,
            Art = StableSpr.cards_LargeBoulder
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
                    thing = new MegaAsteroid
                    {
                        yAnimation = 0.0
                    }
                }         
            ],
            _ => 
            [
                new ASpawn
                {
                    thing = new GiantAsteroid
                    {
                        yAnimation = 0.0
                    }
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
                cost = 1,
                artTint = "ffffff",
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 2,
                artTint = "ffffff",
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}