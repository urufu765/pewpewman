using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Oh No!
/// </summary>
public class CrisisCall : Card, IRegisterable
{
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Rare", "CrisisCall", "name"]).Localize,
            //Art = ModEntry.RegisterSprite(package, "assets/Card/2/TripleTap.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    statusAmount = 5,
                    targetPlayer = true
                }
            ],
            _ => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    statusAmount = 3,
                    targetPlayer = true
                }
            ],
        };
    }


    // public override void OnDiscard(State s, Combat c)
    // {
    //     c.SendCardToExhaust(s, this);
    // }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 0,
                singleUse = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethRare
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethRare
            },
            _ => new CardData
            {
                cost = 1,
                exhaust = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethRare
            }
        };
    }
}