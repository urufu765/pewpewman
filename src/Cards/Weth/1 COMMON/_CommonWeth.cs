namespace Weth.Cards;

/// <summary>
/// Common rarity Weth card
/// </summary>
public class WCCommon : WethCard
{
    public override (Vec pos, Vec size)[] GetGlowSpots()
    {
        return [
            (new(47, 19), new(9, 8)),
            (new(9, 75), new(9, 8))
        ];
    }

    public override (double min, double max)[] GetGlowBrightness(string zoneTag)
    {
        return zoneTag switch
        {
            "zone_first" => [
                (0, 0.1)
            ],
            "zone_lawless" => [
                (0, 0.2)
            ],
            "zone_three" => [
                (0, 0.3)
            ],
            _ => [
                (0, 0)
            ]
        };
    }
}