using System.Collections.Generic;
using System.Reflection;
//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class CargoBlaster : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "CargoBlaster", "name"]).Localize,
            //Art = ModEntry.RegisterSprite(package, "assets/Card/1/Pulsedrive.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ABayBlast
                {
                    wide = true
                }
            ],
            _ => 
            [
                new ABayBlast(),
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
                retain = true,
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 1,
                retain = true,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}