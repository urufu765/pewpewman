using System;
using Microsoft.Extensions.Logging;

namespace Weth.Cards;

/// <summary>
/// Weth Card
/// </summary>
public abstract class WethCard : Card
{
    /// <summary>
    /// The age of the card (mostly used for glow effects)
    /// </summary>
    public double LifeTime { get; set; }

    /// <summary>
    /// Gets the glow spots for the card overlay glow render thing.
    /// </summary>
    /// <returns>Array of glow spots</returns>
    public abstract (Vec pos, Vec size)[] GetGlowSpots();


    /// <summary>
    /// Gets the brightnesses for the glow effect. (When the array size doesn't match the number of glow spots, the first brightness will be used for all the spots.)
    /// </summary>
    /// <param name="zoneTag">Zone name</param>
    /// <returns>Array of glow brightnesses</returns>
    public abstract (double min, double max)[] GetGlowBrightness(string zoneTag);

    /// <summary>
    /// Gets the sprite for the card's overlay.
    /// </summary>
    /// <returns>Card overlay sprite</returns>
    public virtual Spr? GetExtraOverlaySpr() => null;

    /// <summary>
    /// For the extra shines :) (Should be used by Weth cards if need be)
    /// </summary>
    /// <returns>Array of glow spots</returns>
    public virtual (Vec pos, Vec size)[]? GetExtraGlowSpots() => null;

    /// <summary>
    /// Glow brightnesses for the extra shines (same principle as GetGlowBrightness)
    /// </summary>
    /// <param name="zoneTag">Zone name</param>
    /// <returns>Array of glow brightnesses</returns>
    public virtual (double min, double max)[]? GetExtraGlowBrightness(string zoneTag) => null;


    public override void ExtraRender(G g, Vec v)
    {
        LifeTime += g.dt;
        // Render the card's rarity overlay
        try
        {
            if (GetExtraOverlaySpr() is Spr extraOverlaySpr)
            {
                Draw.Sprite(extraOverlaySpr, v.x - 2, v.y - 3);
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Error rendering Weth extra card overlay!");
        }

        // Render the card's overlay glow
        try
        {
            string? zoneTag = g.state?.map?.GetZoneDialogueTag();
            UhDuhHundo.ApplySubtleCrystalOverlayGlow(v, GetGlowSpots(), new("00ffee"), LifeTime, GetGlowBrightness(zoneTag ?? ""), cascade: true, cycleTime: 3, extraSize: new(2, 2));

            if (
                GetExtraGlowSpots() is { } extraSpots &&
                GetExtraGlowBrightness(zoneTag ?? "") is { } extraBrightness
                )
            {
                UhDuhHundo.ApplySubtleCrystalOverlayGlow(v, extraSpots, new("00ffee"), LifeTime, extraBrightness, cascade: true);
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Error rendering Weth card overlay glow!");
        }
    }
}