using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;

namespace Weth;

/// <summary>
/// Helps out with menial tasks
/// </summary>
public class UhDuhHundo
{
    public static ArtifactConfiguration ArtifactRegistrationHelper(Type a, Spr sprite)
    {
        ArtifactMeta? attrs = a.GetCustomAttribute<ArtifactMeta>();
        ArtifactPool[] artpl = attrs?.pools ?? new ArtifactPool[1];
        ArtifactConfiguration ac = new ArtifactConfiguration
        {
            ArtifactType = a,
            Meta = new ArtifactMeta
            {
                owner = ModEntry.Instance.WethDeck.Deck,
                pools = artpl,
                unremovable = attrs is not null && attrs.unremovable,
                extraGlossary = attrs?.extraGlossary ?? []
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", artpl[0].ToString(), a.Name, "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", artpl[0].ToString(), a.Name, "desc"]).Localize,
            Sprite = sprite
        };
        return ac;
    }
}