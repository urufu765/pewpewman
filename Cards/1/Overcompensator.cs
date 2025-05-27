using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// AAAAAA
/// </summary>
public class Overcompensator : WCCommon, IRegisterable
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
                upgradesTo = [Upgrade.A, Upgrade.B],
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "Overcompensator", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/overcompensate.png").Sprite
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
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 6)
                },            
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 3)
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
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2)
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
                cost = 2,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}