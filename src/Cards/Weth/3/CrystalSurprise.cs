using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;
using Weth.Artifacts;

namespace Weth.Cards;

/// <summary>
/// WHAM WHAM!!!
/// </summary>
public class CrystalSurprise : WCRare, IRegisterable, IHasCustomCardTraits
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Rare", "CrystalSurprise", "name"]).Localize,
            Art = StableSpr.cards_PerfectSpecimen
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            _ => 
            [
                new AAddArtifact
                {
                    artifact = new SpaceCrystalFake()
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
                cost = 3,
                exhaust = true,
                buoyant = true,
                artOverlay = ModEntry.Instance.WethRare,
                description = ModEntry.Instance.Localizations.Localize(["Weth", "card", "Rare", "CrystalSurprise", "desc"]),
            },
            Upgrade.A => new CardData
            {
                cost = 2,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethRare,
                description = ModEntry.Instance.Localizations.Localize(["Weth", "card", "Rare", "CrystalSurprise", "desc"]),
            },
            _ => new CardData
            {
                cost = 3,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethRare,
                description = ModEntry.Instance.Localizations.Localize(["Weth", "card", "Rare", "CrystalSurprise", "desc"]),
            }
        };
    }

    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        return new HashSet<ICardTraitEntry>{ModEntry.Instance.KokoroApi.V2.Fleeting.Trait};
    }
}