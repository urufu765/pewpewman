using System.Collections.Generic;
using System.Reflection;
//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class Pulsedrive : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "Pulsedrive", "name"]).Localize,
            //Art = ModEntry.RegisterSprite(package, "assets/Card/1/Pulsedrive.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    targetPlayer = true,
                    statusAmount = 3
                }
            ],
            Upgrade.A => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    targetPlayer = true,
                    statusAmount = 2
                },
            ],
            _ => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.PulseStatus.Status,
                    targetPlayer = true,
                    statusAmount = 1
                },
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 1,
                exhaust = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 0,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}