using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// Flashbang enemy
/// </summary>
public class Discovery : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "Discovery", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/discovery.png").Sprite
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
            _ => 
            [
                ModEntry.Instance.KokoroApi.V2.SpoofedActions.MakeAction(
                    new AAddCard
                    {
                        card = name.ToLower().Contains("crystal")? new CryPlaceholder() : new MechPlaceholder(),
                        destination = CardDestination.Deck,
                        amount = 1,
                    },
                    new AGiveGoodieLikeAGoodBoy()
                ).AsCardAction
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
                artOverlay = ModEntry.Instance.WethUncommon
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 1,
                exhaust = true,
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}