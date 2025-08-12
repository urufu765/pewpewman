using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;
using Weth.Objects;

namespace Weth.Cards;

/// <summary>
/// Shoot two times with MORE EXCITEMENT
/// </summary>
public class ScatterTrash : WCUncommon, IRegisterable
{
    private static Spr altSprite {get; set;}
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Uncommon", "ScatterTrash", "name"]).Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/2/scattertrash.png").Sprite
        });
        altSprite = ModEntry.RegisterSprite(package, "assets/Card/2/scattertrashalt.png").Sprite;
    }


    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.B => 
            [
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = -2
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = -1
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 1
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 2
                }
            ],
            Upgrade.A => 
            [
                new ASpawn
                {
                    thing = new GiantAsteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = -2
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 0
                },
                new ASpawn
                {
                    thing = new GiantAsteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 2
                }
            ],
            _ => 
            [
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = -2
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 0
                },
                new ASpawn
                {
                    thing = new Asteroid
                    {
                        yAnimation = 0.0
                    },
                    offset = 2
                }
            ],
        };
    }


    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.A => new CardData
            {
                cost = 2,
                art = altSprite,
                artTint = "ffffff",
                artOverlay = ModEntry.Instance.WethUncommon
            },
            _ => new CardData
            {
                cost = 2,
                artTint = "ffffff",
                artOverlay = ModEntry.Instance.WethUncommon
            }
        };
    }
}