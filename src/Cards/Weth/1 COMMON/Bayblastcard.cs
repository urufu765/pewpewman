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
public class CargoBlaster : Card, IRegisterable
{
    private static Spr nonFlipSprite {get; set;}
    private static Spr flipSprite {get; set;}

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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Common", "CargoBlaster", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/1/bayblast.png").Sprite
        });
        nonFlipSprite = ModEntry.RegisterSprite(package, "assets/Card/1/bayblasta.png").Sprite;
        flipSprite = ModEntry.RegisterSprite(package, "assets/Card/1/bayblastb.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B =>
            [
                new ABayBlastV2
                {
                    disabled = flipped
                },
                new ABayBlastV2
                {
                    flared = true,
                    disabled = !flipped,
                },
            ],
            Upgrade.A => 
            [
                new ABayBlastV2
                {
                    range = 2
                }
            ],
            _ => 
            [
                new ABayBlastV2(),
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
                retain = true,
                floppable = true,
                art = flipped ? flipSprite : nonFlipSprite,
                artOverlay = ModEntry.Instance.WethCommon
            },
            _ => new CardData
            {
                cost = 1,
                retain = true,
                artOverlay = ModEntry.Instance.WethCommon
            }
        };
    }
}