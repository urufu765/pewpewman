using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Shoot powerfully
/// </summary>
public class Powershot : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "Powershot", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/powershot.png").Sprite
        });
        altSprite = ModEntry.RegisterSprite(package, "assets/Card/2/powershotalt.png").Sprite;
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
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    statusAmount = 1,
                    targetPlayer = true
                },
                new AStatus
                {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                }
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
                new AStatus
                {
                    status = Status.overdrive,
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
                    status = Status.overdrive,
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
            Upgrade.A or Upgrade.B => new CardData
            {
                cost = 1,
                art = altSprite,
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 1,
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}