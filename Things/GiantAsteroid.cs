using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nickel;
using Weth.Actions;

namespace Weth.Objects;

public class GiantAsteroid : Asteroid
{
    public override Spr? GetIcon()
    {
        return ModEntry.Instance.SprGiantAsteroidIcon;
    }

    public override double GetWiggleAmount()
    {
        return 0.75;
    }

    public override double GetWiggleRate()
    {
        return 0.75;
    }

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> tooltips =
        [
            new GlossaryTooltip("objecttooltip.giantasteroid")
            {
                Icon = ModEntry.Instance.SprGiantAsteroidIcon,
                IsWideIcon = true,
                Title = ModEntry.Instance.Localizations.Localize(["object", "GiantAsteroid", "name"]),
                TitleColor = Colors.midrow,
                Description = ModEntry.Instance.Localizations.Localize(["object", "GiantAsteroid", "desc"]),
            },
            .. base.GetTooltips(),
        ];
        return tooltips;
    }

    public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
        List<CardAction> actions = new();
        List<CardAction>? based = base.GetActionsOnDestroyed(s, c, wasPlayer, worldX);
        actions.Add(new ASpawnFromMidrow
        {
            worldX = x,
            thing = new Asteroid(),
            offset = -1,
            byPlayer = wasPlayer,
        });
        actions.Add(new ASpawnFromMidrow
        {
            worldX = x,
            thing = new Asteroid(),
            offset = 1,
            byPlayer = wasPlayer
        });
        if (based is not null)
        {
            actions.AddRange(based); 
        }
        return actions;
    }

    public override void Render(G g, Vec v)
    {
        DrawWithHilight(g, ModEntry.Instance.SprGiantAsteroid, v + GetOffset(g, false), Mutil.Rand(x + 0.1) > 0.5, Mutil.Rand(x + 0.2) > 0.5);
    }
}