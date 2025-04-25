using System.Collections.Generic;
using System.Reflection;
//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class PulsedriveCard : Card, IRegisterable
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
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/pulsedrive.png").Sprite
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
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    targetPlayer = true,
                    statusAmount = 2
                },
                new AAddCard
                {
                    card = new PulsedriveCard{
                        upgrade = Upgrade.B,
                        temporaryOverride = true
                    },
                    amount = 2,
                    destination = CardDestination.Discard
                }
            ],
            Upgrade.A => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    targetPlayer = true,
                    statusAmount = 2
                },
                new AAddCard
                {
                    card = new PulsedriveCard{
                        temporaryOverride = true
                    },
                    destination = CardDestination.Discard
                }
            ],
            _ => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
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
            Upgrade.A => new CardData
            {
                cost = 0,
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