using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Oh No!
/// </summary>
public class CrisisCall : WCUncommon, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Uncommon", "CrisisCall", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/3/crisiscall.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        string name = "";
        if (c.otherShip?.ai?.character?.type is not null)
        {
            name = c.otherShip.ai.character.type;
        }
        return upgrade switch
        {
            Upgrade.B => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = 3,
                    targetPlayer = true,
                    dialogueSelector = ".crisisCallCulled"
                },
                new AAddCard
                {
                    card = new PulsedriveCard
                    {
                        exhaustOverride = true,
                        exhaustOverrideIsPermanent = true
                    },
                    amount = 2,
                    destination = CardDestination.Discard
                }
            ],
            _ => 
            [
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = 2,
                    targetPlayer = true
                }
            ],
        };
    }


    // public override void OnDiscard(State s, Combat c)
    // {
    //     c.SendCardToExhaust(s, this);
    // }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 0,
                singleUse = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                retain = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 0,
                exhaust = true,
                artTint = "4ab3ff",
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}