using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Shoot a disabling round
/// </summary>
public class Disabler : WCCommon, IRegisterable, IHasCustomCardTraits
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
                    brittle = true
                },
                new AAddCard
                {
                    card = new Disabler(),
                    destination = CardDestination.Exhaust,
                }
            ],
            Upgrade.A =>
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
                    destination = CardDestination.Exhaust,
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
            Upgrade.B or Upgrade.A => new CardData
            {
                cost = 1,
                singleUse = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 0,
                exhaust = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
    
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        if (upgrade == Upgrade.B) return new HashSet<ICardTraitEntry> { ModEntry.Instance.KokoroApi.V2.Fleeting.Trait };
        return new HashSet<ICardTraitEntry>();
    }
}