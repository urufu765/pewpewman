using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Shoot a disabling round
/// </summary>
public class Disabler : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "Disabler", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/disabler.png").Sprite
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
                    damage = GetDmg(s, 0),
                    stunEnemy = true,
                    weaken = true
                },
                new AAddCard
                {
                    card = new Disabler(),
                    destination = CardDestination.Discard,
                }
            ],
            _ => 
            [
                new ASplitshot
                {
                    damage = GetDmg(s, 0),
                    stunEnemy = true
                }
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
                singleUse = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 1,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}