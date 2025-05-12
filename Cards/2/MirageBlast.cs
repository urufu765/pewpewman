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
public class MirageBlast : Card, IRegisterable
{
    private static Spr normalSprite;
    private static Spr flippedSprite;
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "MirageBlast", "name"]).Localize
        });
        normalSprite = ModEntry.RegisterSprite(package, "assets/Card/2/mirageblast.png").Sprite;
        flippedSprite = ModEntry.RegisterSprite(package, "assets/Card/2/mirageblastflipped.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B =>
            [
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true,
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = 2,
                    isTeleport = true
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    stunEnemy = true
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true
                }
            ],
            Upgrade.A =>
            [
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true,
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    piercing = true
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = 2,
                    isTeleport = true
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    piercing = true
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true
                }
            ],
            _ => 
            [
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true,
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = 2,
                    isTeleport = true
                },
                new ASplitshot
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                },
                new AMove
                {
                    targetPlayer = true,
                    dir = -1,
                    isTeleport = true
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
                art = flipped? flippedSprite : normalSprite,
                flippable = true,
                artTint = "ffc47b",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            Upgrade.A => new CardData
            {
                cost = 1,
                art = flipped? flippedSprite : normalSprite,
                artTint = "ea4a4a",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 1,
                art = flipped? flippedSprite : normalSprite,
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}