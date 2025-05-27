namespace Weth.Cards;

/// <summary>
/// Rare rarity Weth card
/// </summary>
public class WCRare : WethCard
{
    public override (Vec pos, Vec size)[] GetGlowSpots()
    {
        return [
            (new(52, 21), new(7, 9)),   // 2
            (new(5, 38), new(7, 5)),    // 4
            (new(53, 54), new(10, 15)), // 7
            (new(47, 19), new(9, 10)),  // 1
            (new(4, 51), new(3, 5)),    // 8
            (new(9, 74), new(13, 12)),  // 11
            (new(54, 44), new(6, 9)),   // 6
            (new(3, 57), new(3, 5)),    // 9
            (new(54, 34), new(7, 12)),  // 5
            (new(5, 33), new(8, 7)),    // 3
            (new(5, 68), new(8, 9)),    // 10
        ];
    }

    public override (double min, double max)[] GetGlowBrightness(string zoneTag)
    {
        return zoneTag switch
        {
            "zone_first" => [
                (0, 0.35),  // 2
                (0, 0.25),  // 4
                (0, 0.3),   // 7
                (0, 0.35),  // 1
                (0, 0.15),  // 8
                (0, 0.4),   // 11
                (0, 0.25),  // 6
                (0, 0.1),   // 9
                (0, 0.25),  // 5
                (0, 0.25),  // 3
                (0, 0.35),  // 10
            ],
            "zone_lawless" => [
                (0, 0.42),  // 2
                (0, 0.3),   // 4
                (0, 0.35),  // 7
                (0, 0.4),   // 1
                (0, 0.2),   // 8
                (0, 0.5),   // 11
                (0, 0.3),   // 6
                (0, 0.15),  // 9
                (0, 0.3),   // 5
                (0, 0.3),   // 3
                (0, 0.42),  // 10
            ],
            "zone_three" => [
                (0, 0.5),   // 2
                (0, 0.35),  // 4
                (0, 0.4),   // 7
                (0, 0.55),  // 1
                (0, 0.25),  // 8
                (0, 0.6),   // 11
                (0, 0.35),  // 6
                (0, 0.2),   // 9
                (0, 0.35),  // 5
                (0, 0.35),  // 3
                (0, 0.5),   // 10
            ],
            _ => [
                (0, 0)
            ]
        };
    }
}