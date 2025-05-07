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
public class SplitshotCard : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Common", "Splitshot", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/splitshot.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 2),
                    piercing = true
                }
            ],
            Upgrade.A => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 3)
                }
            ],
            _ => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 2)
                },
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