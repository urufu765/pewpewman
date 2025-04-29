using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Nanoray.PluginManager;
using Nickel;
using OneOf.Types;

namespace Weth.Cards;

/// <summary>
/// CRYSTAL: Attack
/// </summary>
public class CryShield : Card, IRegisterable
{
    public int Age {get;set;} = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.GoodieDeck.Deck,
                rarity = Rarity.rare,
                dontOffer = true,
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "Shoulder", "name"]).Localize,
            Art = StableSpr.cards_BoostCapacitors
        });
    }


    public override void OnDraw(State s, Combat c)
    {
        Age++;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            _ => 
            [
                new AShieldMax
                {
                    amount = 1,
                    targetPlayer = true
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
                artOverlay = ModEntry.Instance.GoodieCrystalA,
                singleUse = true,
                buoyant = true,
                artTint = "6284ff"
            }
        };
    }
}