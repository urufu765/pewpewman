using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Can't stop!
/// </summary>
public class UnstoppableForce : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Rare", "UnstoppableForce", "name"]).Localize,
            //Art = ModEntry.RegisterSprite(package, "assets/Card/2/TripleTap.png").Sprite
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
                    damage = GetDmg(s, 1),
                    piercing = true
                },
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
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
                cost = 1,
                infinite = true,
                retain = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethRare
            },
            _ => new CardData
            {
                cost = 1,
                infinite = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethRare
            }
        };
    }
}