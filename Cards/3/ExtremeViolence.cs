using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// WHAM WHAM!!!
/// </summary>
public class ExtremeViolence : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Rare", "ExtremeViolence", "name"]).Localize,
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
                    damage = GetDmg(s, 0),
                    stunEnemy = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
                },
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    piercing = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
                    piercing = true
                },
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
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
                exhaust = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethRare
            },
            _ => new CardData
            {
                cost = 3,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethRare
            }
        };
    }
}