using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Flashbang enemy
/// </summary>
public class Discovery : WCUncommon, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "Discovery", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/discovery.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new AGiveGoodieLikeAGoodBoy
                {
                    asAnOffering = true,
                    amount = 2,
                    betterOdds = true,
                }
            ],
            Upgrade.A => 
            [
                new AGiveGoodieLikeAGoodBoy
                {
                    asAnOffering = true,
                    betterOdds = true,
                    amount = 3,
                    upgrade = Upgrade.A
                }
            ],
            _ => 
            [
                new AGiveGoodieLikeAGoodBoy
                {
                    asAnOffering = true,
                    amount = 3
                }
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 0,
                artOverlay = ModEntry.Instance.WethUncommon,
                description = string.Format(ModEntry.Instance.Localizations.Localize(["card", "Uncommon", "Discovery", "desc"]), 2),
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethUncommon,
                description = string.Format(ModEntry.Instance.Localizations.Localize(["card", "Uncommon", "Discovery", "descA"]), 3),
            },
            _ => new CardData
            {
                cost = 0,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethUncommon,
                description = string.Format(ModEntry.Instance.Localizations.Localize(["card", "Uncommon", "Discovery", "desc"]), 3),
            }
        };
    }
}