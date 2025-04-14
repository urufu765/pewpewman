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
                    stunEnemy = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
                    fast = true
                },
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    piercing = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
                    piercing = true,
                    fast = true
                },
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 4),
                    fast = true
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