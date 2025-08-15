
using System.Collections.Generic;
using FSPRO;
using Nickel;

namespace Weth.Actions;

public class ASodaDescription : ADummyAction
{
    public required string description;
    
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = [];
        tooltips.Add(new GlossaryTooltip($"uselessWethCardAction.MilkSodaDescriptor")
        {
            Description = description,
        });
        tooltips.Add(new TTDivider());
        tooltips.AddRange(base.GetTooltips(s));
        return tooltips;
    }
}