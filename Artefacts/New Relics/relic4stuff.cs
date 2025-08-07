using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using Nickel;

namespace Weth.Artifacts;


public abstract class WethRelicFour : Artifact
{
    public int Amount { get; set; }
    public bool Special { get; set; }
    public string? firstStack;

    public virtual void GainStack(State state, bool? special = null)
    {
        Amount++;
        if (special is not null)
        {
            Special = special.Value;
        }
    }

    public override int? GetDisplayNumber(State s)
    {
        return Amount;
    }

    public override Spr GetSprite()
    {
        return ModEntry.Instance.NewRelicSprites[GetType()];
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new GlossaryTooltip($"wethRelicFlavourText.{GetType().Name}")
        {
            Description = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", GetType().Name, "flavour"])
        }];
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        if (ModEntry.Instance.NewRelicStatuses.ContainsKey(GetType()))
        {
            state.ship.Set(ModEntry.Instance.NewRelicStatuses[GetType()], Amount);
        }
    }

    public override void OnReceiveArtifact(State state)
    {
        if (firstStack is not null)
        {
            GainStack(state);
        }
    }
}

public abstract class WethRelicFourFake : Artifact
{
    public abstract Type RealRelicType { get; }
    public virtual bool Special { get; set; }

    public override void OnReceiveArtifact(State state)
    {
        if (!state.EnumerateAllArtifacts().Any(a => a.GetType() == RealRelicType) && Activator.CreateInstance(RealRelicType) is WethRelicFour wrf)
        {
            wrf.firstStack = GetType().Name;
            wrf.Special = Special;
            state.GetCurrentQueue().QueueImmediate(new AAddArtifact
            {
                artifact = wrf
            });
        }
        else if (state.EnumerateAllArtifacts().Find(a => a.GetType() == RealRelicType) is WethRelicFour wrf2)
        {
            wrf2.GainStack(state);
        }
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        string extra = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", RealRelicType.Name, Special? "extras_special":"extras"]);

        if (extra.Length == 0) return null;

        return [new GlossaryTooltip($"wethRelicExtraText.{GetType().Name}")
        {
            Description = extra
        }];
    }
}

public static class WethRelicFourHelpers
{
    public static void DontAddFakeRelic(State __instance, Artifact r)
    {
        if (r is WethRelicFourFake)
        {
            UhDuhHundo.ArtifactRemover(__instance, r);
        }
    }

    public static void FixTheTooltips(ref List<Tooltip> __result, Artifact __instance)
    {
        if (__instance is WethRelicFourFake wrff)
        {
            if (MG.inst?.g?.state?.EnumerateAllArtifacts().Find(a => a.GetType() == wrff.RealRelicType) is WethRelicFour wrf)
            {
                __result[0] = RelicTooltip(wrff.RealRelicType, wrf.Amount + 1, false, special:wrff.Special);
            }
            else
            {
                __result[0] = RelicTooltip(wrff.RealRelicType, icon: false, special: wrff.Special);
            }
        }
        else if (__instance is WethRelicFour wrfx)
        {
            __result[0] = RelicTooltip(wrfx.GetType(), wrfx.Amount, false, special:wrfx.Special);
        }
    }


    public static Tooltip RelicTooltip(Type relic, int n = 1, bool icon = true, bool micro = false, bool special = false)
    {
        if (micro) // Make it so that event relics ignore the numbering maybe
        {
            return new TTTTTTGlossary($"wethRelicQuickie.{relic.Name}")
            {
                Title = $"<c=keyword>{n}</c> <c=artifact>{ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, "title"])}</c>",
                Icon = icon ? ModEntry.Instance.NewRelicIcons[relic] : null
            };
        }
        return new GlossaryTooltip($"wethRelicTooltip.{relic.Name}")
        {
            Icon = icon ? ModEntry.Instance.NewRelicIcons[relic] : null,
            Title = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, "name"]),
            TitleColor = Colors.artifact,
            Description = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, special? "desc_special":"desc"]),
            vals = [n]
        };
    }
}