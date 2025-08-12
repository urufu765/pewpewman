using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// CRYSTAL: Attack
/// </summary>
public class CryPlaceholder : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.GoodieDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A],
                dontOffer = true,
                unreleased = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "Placeholder", "name"]).Localize,
            Art = StableSpr.cards_colorless
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            _ => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.UnknownStatus.Status,
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
                artOverlay = ModEntry.Instance.GoodieCrystalA,
                exhaust = true,
                retain = true,
                temporary = true,
                artTint = "6284ff"
            },
            _ => new CardData
            {
                cost = 0,
                artOverlay = ModEntry.Instance.GoodieCrystal,
                singleUse = true,
                temporary = true,
                artTint = "6284ff"
            }
        };
    }
}