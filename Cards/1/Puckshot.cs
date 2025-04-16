using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shot for stun
/// </summary>
public class Puckshot : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "Puckshot", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/puckshot.png").Sprite
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
                    status = Status.stunCharge,
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
                    status = Status.stunCharge,
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
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 1,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}