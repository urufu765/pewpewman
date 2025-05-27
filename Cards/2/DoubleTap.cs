using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shoot two times
/// </summary>
public class DoubleTap : WCUncommon, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "DoubleTap", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/doubletap.png").Sprite
        });
        altSprite = ModEntry.RegisterSprite(package, "assets/Card/2/doubletapalt.png").Sprite;
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
                    damage = GetDmg(s, 4),
                    stunEnemy = true,
                    fast = true
                }            
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
                    damage = GetDmg(s, 4),
                    piercing = true,
                    fast = true
                }
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
                    damage = GetDmg(s, 4),
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