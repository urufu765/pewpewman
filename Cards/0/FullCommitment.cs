using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Artifacts;

namespace Weth.Cards;

/// <summary>
/// Token card for All Power To Cannons
/// </summary>
public class FullCommitment : Card, IRegisterable
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
                upgradesTo = [],
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "FullCommit", "name"]).Localize,
            Art = StableSpr.cards_ColorlessTrash
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        int x = s.ship.Get(Status.shield);
        x += s.ship.Get(Status.evade);
        return upgrade switch
        {
            _ =>
            [
                new AStatus
                {
                    status = Status.lockdown,
                    statusAmount = 5,
                    targetPlayer = true,
                },
                new AVariableHint
                {
                    status = Status.shield,
                    secondStatus = Status.evade
                },
                new AStatus
                {
                    status = ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive,
                    statusAmount = x,
                    targetPlayer = true,
                    xHint = 1
                },
                new AStatus
                {
                    status = Status.shield,
                    statusAmount = 0,
                    targetPlayer = true,
                    mode = AStatusMode.Set
                },
                new AStatus
                {
                    status = Status.evade,
                    statusAmount = 0,
                    targetPlayer = true,
                    mode = AStatusMode.Set
                }
            ],
        };
    }



    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = 0,
                singleUse = true,
                temporary = true,
                retain = true
            }
        };
    }
}