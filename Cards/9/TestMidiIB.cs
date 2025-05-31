using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

//using Weth.Features;
using Nanoray.PluginManager;
using Nickel;
using Weth.Actions;

namespace Weth.Cards;

/// <summary>
/// gives Pulsedrive
/// </summary>
public class PlayIncompetentBaffoon : WCCommon, IRegisterable
{
    public IModSoundInstance? IncompetentBaffoon { get; set; }

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.WethDeck.Deck,
                rarity = Rarity.common,
                unreleased = true,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Token", "IncompetentB", "name"]).Localize
        });
    }


    public override void OnFlip(G g)
    {
        if (flipped)
        {
            if (IncompetentBaffoon is IModSoundInstance imsi && Helpers.CheckSoundValidity(imsi))
            {
                imsi.IsPaused = false;
            }
            else
            {
                IncompetentBaffoon = ModEntry.Instance.MidiTestIncompetentB.CreateInstance();
            }
        }
        else
        {
            if (IncompetentBaffoon is IModSoundInstance imsi)
            {
                if (Helpers.CheckSoundValidity(imsi))
                {
                    imsi.IsPaused = true;
                }
                else
                {
                    IncompetentBaffoon = null;
                }
            }
        }
    }


    public override CardData GetData(State state)
    {
        return new CardData
        {
            floppable = true,
            artOverlay = ModEntry.Instance.WethCommon,
            description = ModEntry.Instance.Localizations.Localize(["card", "Token", "JourneyV", flipped ? "pause" : "play"]),
            unplayable = true
        };
    }
}
