using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// Flashbang enemy
/// </summary>
public class Bloom : WCUncommon, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Uncommon", "Bloom", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/bloom.png").Sprite
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
                    damage = GetDmg(s, 1),
                    stunEnemy = true
                },
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
            _ => 
            [
                new AStatus
                {
                    status = Status.stunCharge,
                    statusAmount = 1,
                    targetPlayer = true
                },
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Minidrive,
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
            Upgrade.A => new CardData
            {
                cost = 1,
                infinite = true,
                retain = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 1,
                infinite = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}