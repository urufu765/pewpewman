using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// AAAAAA
/// </summary>
public class Overcompensator : Card, IRegisterable
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
            //Art = ModEntry.RegisterSprite(package, "assets/Card/1/TripleTap.png").Sprite
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
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 2)
                },
                new AAttack
                {
                    damage = GetDmg(s, 3)
                },            
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
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