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
public class FeralBlast : Card, IRegisterable
{
    private static Spr closeSprite {get; set;}
    private static Spr openSprite {get; set;}

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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "FeralBlast", "name"]).Localize,
            Art = StableSpr.cards_ColorlessTrash,
        });
    
        openSprite = ModEntry.RegisterSprite(package, "assets/Card/2/feralblast.png").Sprite;
        closeSprite = ModEntry.RegisterSprite(package, "assets/Card/2/feralblastalt.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        int x = c.energy;
        return upgrade switch
        {
            Upgrade.B =>
            [
                ModEntry.Instance.KokoroApi.V2.EnergyAsStatus.MakeVariableHint().AsCardAction,
                new ABayBlastV2
                {
                    disabled = flipped,
                    xHint = 2,
                    range = x * 2
                },
                new ABayBlastV2
                {
                    disabled = !flipped,
                    flared = true,
                    xHint = 2,
                    range = x * 2
                },
                ModEntry.Instance.KokoroApi.V2.EnergyAsStatus.MakeStatusAction(0, AStatusMode.Set).AsCardAction
            ],
            _ => 
            [
                ModEntry.Instance.KokoroApi.V2.EnergyAsStatus.MakeVariableHint().AsCardAction,
                new ABayBlastV2
                {
                    disabled = flipped,
                    xHint = 1,
                    range = x
                },
                new ABayBlastV2
                {
                    disabled = !flipped,
                    flared = true,
                    xHint = 1,
                    range = x
                },
                ModEntry.Instance.KokoroApi.V2.EnergyAsStatus.MakeStatusAction(0, AStatusMode.Set).AsCardAction
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.B => new CardData
            {
                cost = 0,
                exhaust = true,
                floppable = true,
                art = flipped ? closeSprite : openSprite,
                artOverlay = ModEntry.Instance.WethUncommon
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                retain = true,
                floppable = true,
                art = flipped ? closeSprite : openSprite,
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 0,
                exhaust = true,
                floppable = true,
                art = flipped ? closeSprite : openSprite,
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}