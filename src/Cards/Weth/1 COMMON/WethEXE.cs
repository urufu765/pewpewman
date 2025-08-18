using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Weth.Cards;

/// <summary>
/// A card that attempts to build a cure, creating The Cure and The Failure cards
/// </summary>
public class WethExe : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["Weth", "card", "Common", "WethEXE", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/0/EXE.png").Sprite
        });
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ACardOffering
                {
                    amount = 3,
                    limitDeck = ModEntry.Instance.WethDeck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                    dialogueSelector = ".summonWeth"
                }
            ],
            _ => 
            [
                new ACardOffering
                {
                    amount = 2,
                    limitDeck = ModEntry.Instance.WethDeck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                    dialogueSelector = ".summonWeth"
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
                exhaust = true,
                description = ColorlessLoc.GetDesc(state, 3, ModEntry.Instance.WethDeck.Deck),
                artTint = "125f66"
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                description = ColorlessLoc.GetDesc(state, 2, ModEntry.Instance.WethDeck.Deck),
                artTint = "125f66"
            },
            _ => new CardData
            {
                cost = 1,
                exhaust = true,
                description = ColorlessLoc.GetDesc(state, 2, ModEntry.Instance.WethDeck.Deck),
                artTint = "125f66"
            }
        };
    }
}