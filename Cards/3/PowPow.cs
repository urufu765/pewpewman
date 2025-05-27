using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// WHAM WHAM!!!
/// </summary>
public class PowPow : WCRare, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Rare", "PowPow", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/3/powpow.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    stunEnemy = true,
                    piercing = true
                },
                new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true
                },
                new AAddCard
                {
                    card = new Powershot(),
                    amount = 2,
                    destination = CardDestination.Discard
                }
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    stunEnemy = true,
                    piercing = true
                },
                new AStatus
                {
                    status = Status.powerdrive,
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
            Upgrade.B => new CardData
            {
                cost = 1,
                singleUse = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethRare
            },
            Upgrade.A => new CardData
            {
                cost = 3,
                exhaust = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethRare
            },
            _ => new CardData
            {
                cost = 4,
                exhaust = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethRare
            }
        };
    }
}