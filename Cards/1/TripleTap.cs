using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shoot three times
/// </summary>
public class TripleTap : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "TripleTap", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/tripletap.png").Sprite
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
                    piercing = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true,
                    fast = true
                },            
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 1),
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
                    damage = GetDmg(s, 1),
                    fast = true
                },
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = 1,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}