using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Shoot two times with MORE EXCITEMENT
/// </summary>
public class Spreadshot : Card, IRegisterable
{
    private static Spr altSprite {get; set;}
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "Spreadshot", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/spreadshot.png").Sprite
        });
        altSprite = ModEntry.RegisterSprite(package, "assets/Card/2/spreadshotalt.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 2)
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 2)
                }            
            ],
            Upgrade.A => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 3)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
                }
            ],
            _ => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 2)
                },
                new AAttack
                {
                    damage = GetDmg(s, 1)
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
                art = altSprite,
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