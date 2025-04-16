using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shoot two times
/// </summary>
public class DoubleTap : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "DoubleTap", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/doubletap.png").Sprite
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
                    damage = GetDmg(s, 2),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    stunEnemy = true,
                    fast = true
                }            
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true,
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true,
                    fast = true
                }
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                },
                new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
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
                cost = 2,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 2,
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}