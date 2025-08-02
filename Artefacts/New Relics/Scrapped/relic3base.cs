using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nickel;
#if false
namespace Weth.Artifacts;

[Obsolete("Container won't be used")]
[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public abstract class NewWethSpaceRelics : Artifact
{
    public WethRelics GetThing()
    {
        return (Attribute.GetCustomAttribute(GetType(), typeof(RelicMeta))! as RelicMeta)!.theRelic;
    }

    public override void OnReceiveArtifact(State state)
    {
        if (!state.EnumerateAllArtifacts().Any(a => a is RelicCollection))
        {
            state.GetCurrentQueue().QueueImmediate(new AAddArtifact
            {
                artifact = new RelicCollection()
            });
        }
        state.GetCurrentQueue().Queue(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}"
        });
    }

    public override void OnRemoveArtifact(State state)
    {
        if (state.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc)
        {
            rc.ObtainRelic(GetThing());
        }
    }

    /// <summary>
    /// Do an emergency hook activation in situations where the relic isn't disposed of correctly (and added to the collection), only if there's an OnCombatStart 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="combat"></param>
    public override void OnCombatStart(State state, Combat combat)
    {
        if (RelicCollection.RelicOnCombatStart.Keys.Contains(GetThing()) && state.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc)
        {
            RelicCollection.RelicOnCombatStart[GetThing()](rc, state, combat, rc.GetRelicCount(GetThing()) + 1);
            // state.GetCurrentQueue().QueueImmediate(new ALoseArtifact
            // {
            //     artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}"
            // });
        }
    }

    /// <summary>
    /// Do an emergency hook activation and removal in situations where the relic isn't disposed of correctly.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="combat"></param>
    public override void OnTurnStart(State state, Combat combat)
    {
        if (RelicCollection.RelicOnTurnStart.Keys.Contains(GetThing()) && state.EnumerateAllArtifacts().Find(a => a is RelicCollection) is RelicCollection rc)
        {
            RelicCollection.RelicOnTurnStart[GetThing()](rc, state, combat, rc.GetRelicCount(GetThing()) + 1);
        }
        state.GetCurrentQueue().QueueImmediate(new ALoseArtifact
        {
            artifactType = $"{ModEntry.Instance.UniqueName}::{GetType().Name}"
        });
    }

    // TODO: Make this less reliant on the Collection artifact.
    public static RelicData? GetRelicData(RelicCollection collection, Type relic)
    {
        WethRelics r = (Attribute.GetCustomAttribute(relic, typeof(RelicMeta))! as RelicMeta)!.theRelic;
        if (collection.RelicSaveData.TryGetValue(r, out RelicData? value))
        {
            return value;
        }
        return null;
    }

    public static void OverwriteRelicData(RelicCollection collection, Type relic, RelicData newData)
    {
        collection.RelicSaveData[(Attribute.GetCustomAttribute(relic, typeof(RelicMeta))! as RelicMeta)!.theRelic] = newData;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new GlossaryTooltip($"wethRelicFlavourText.{GetThing()}")
        {
            Description = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", GetThing().ToString(), "flavour"])
        }];
    }

    public static Tooltip RelicTooltip(Type relic, int n = 1, bool icon = true, bool micro = false)
    {
        if (micro) // Make it so that event relics ignore the numbering maybe
        {
            return new TTTTTTGlossary($"wethRelicQuickie.{relic.Name}")
            {
                Title = $"<c=keyword>{n}</c> <c=artifact>{ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, "title"])}</c>",
                Icon = icon ? ModEntry.Instance.NewRelicIcons[(Attribute.GetCustomAttribute(relic, typeof(RelicMeta))! as RelicMeta)!.theRelic] : null
            };
        }
        return new GlossaryTooltip($"wethRelicTooltip.{relic.Name}")
        {
            Icon = icon ? ModEntry.Instance.NewRelicIcons[(Attribute.GetCustomAttribute(relic, typeof(RelicMeta))! as RelicMeta)!.theRelic] : null,
            Title = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, "name"]),
            TitleColor = Colors.artifact,
            Description = ModEntry.Instance.Localizations.Localize(["artifact", "Unreleased", relic.Name, "desc"]),
            vals = [n]
        };
    }
}
#endif