using System.Collections.Generic;
using System.Reflection;
//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class DoubleBlast : Card, IRegisterable
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
                upgradesTo = [Upgrade.A, Upgrade.B],
                unreleased = true,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "DoubleBlast", "name"]).Localize,
            Art = StableSpr.cards_ColorlessTrash
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
                    damage = GetDmg(s, 1)
                },
                new ABayBlast
                {
                    wide = true
                }
            ],
            Upgrade.A => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 1)
                },
                new ABayBlast(),
            ],
            _ => 
            [
                new AAttack
                {
                    damage = GetDmg(s, 1)
                },
                new ABayBlast(),
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            _ => new CardData
            {
                cost = 1,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}