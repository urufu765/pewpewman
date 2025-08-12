namespace Weth.Cards;

/// <summary>
/// Uncommon rarity Weth card
/// </summary>
public class WCUncommon : WethCard
{
    public override (Vec pos, Vec size)[] GetGlowSpots()
    {
        return [
            (new(52, 19), new(5, 5)),  // 2
            (new(9, 75), new(9, 7)),   // 9
            (new(55, 32), new(3, 5)),  // 4
            (new(53, 55), new(7, 9)),  // 7
            (new(47, 19), new(9, 8)),  // 1
            (new(3, 34), new(3, 4)),   // 3
            (new(55, 40), new(3, 3)),  // 5
            (new(6, 73), new(6, 7)),   // 8
            (new(3, 69), new(3, 3)),   // 6
        ];
    }


    public override (double min, double max)[] GetGlowBrightness(string zoneTag)
    {
        return zoneTag switch
        {
            "zone_first" => [
                (0, 0.3),   // 2
                (0, 0.35),  // 9
                (0, 0.1),   // 4
                (0, 0.1),   // 7
                (0, 0.35),  // 1
                (0, 0.1),   // 3
                (0, 0.1),   // 5
                (0, 0.25),  // 8
                (0, 0.1),   // 6
            ],
            "zone_lawless" => [
                (0, 0.35),  // 2
                (0, 0.4),   // 9
                (0, 0.2),   // 4
                (0, 0.2),   // 7
                (0, 0.4),   // 1
                (0, 0.2),   // 3
                (0, 0.2),   // 5
                (0, 0.3),   // 8
                (0, 0.2),   // 6
            ],
            "zone_three" => [
                (0, 0.5),   // 2
                (0, 0.55),  // 9
                (0, 0.3),   // 4
                (0, 0.3),   // 7
                (0, 0.55),  // 1
                (0, 0.3),   // 3
                (0, 0.3),   // 5
                (0, 0.45),  // 8
                (0, 0.3),   // 6
            ],
            _ => [
                (0, 0)
            ]
        };
    }
}