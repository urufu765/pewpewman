using System.Collections.Generic;
using Nickel;

namespace Weth.Actions;

public class APseudoPulsedriveGiver : AStatus
{
    public List<(string, string)>? descriptions;

    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = [];
        foreach ((string key, string stuff) description in descriptions??[])
        {
            tooltips.Add(new GlossaryTooltip($"statustooltip.{description.key}")
            {
                Description = description.stuff,
            });
        }
        tooltips.AddRange(base.GetTooltips(s));
        return tooltips;
    }
} 