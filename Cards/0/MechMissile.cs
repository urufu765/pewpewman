using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// MECH: Defence
/// </summary>
public class MechMissile : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.GoodieDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A],
                dontOffer = true,
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "Mizzile", "name"]).Localize,
            Art = StableSpr.cards_SeekerMissileCard
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            _ => 
            [
                new ASpawn
                {
                    thing = new Missile
                    {
                        missileType = MissileType.heavy
                    }
                }
            ],
        };
    }



    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.A => new CardData
            {
                cost = 0,
                artOverlay = ModEntry.Instance.GoodieMechA,
                exhaust = true,
                retain = true,
                temporary = true,
                artTint = "a0a0a0"
            },
            _ => new CardData
            {
                cost = 0,
                artOverlay = ModEntry.Instance.GoodieMech,
                singleUse = true,
                temporary = true,
                artTint = "a0a0a0"
            }
        };
    }
}