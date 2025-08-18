using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

public class HeatedShot : TarmaucCard, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        Rarity rare = Rarity.common;
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                rarity = rare,
                upgradesTo = [Upgrade.A, Upgrade.B],
                deck = ModEntry.Instance.RoadkillDeck.Deck
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Roadkill", "card", rare.ToString(), MethodBase.GetCurrentMethod()!.DeclaringType!.Name, "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, $"assets/card/5/{MethodBase.GetCurrentMethod()!.DeclaringType!.Name}.png").Sprite
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
                    status = Status_burn,
                    statusAmount = 1
                },
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    status = Status.heat,
                    statusAmount = 1
                },
                new AStatus
                {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true
                },
            ],
            Upgrade.A => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1),
                    status = Status_burn,
                    statusAmount = 1
                }
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 0),
                    status = Status_burn,
                    statusAmount = 1
                }
            ]
        };
    }

    public override CardData GetPreData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = 1
            }
        };
    }
}