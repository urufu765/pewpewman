using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nickel;

namespace Weth;

/// <summary>
/// Helps out with menial tasks
/// </summary>
public static class UhDuhHundo
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

    public static void ArtifactRemover(ref State state, string artifactName)
    {
        string artifactType = $"{ModEntry.Instance.UniqueName}::{artifactName}";
        foreach (Character character in state.characters)
        {
            if (character.deckType == ModEntry.Instance.WethDeck.Deck)
            {
                foreach (Artifact artifact in character.artifacts)
                {
                    if (artifact.Key() == artifactType)
                    {
                        artifact.OnRemoveArtifact(state);
                    }
                }
                character.artifacts.RemoveAll(a => a.Key() == artifactType);
            }
        }
    }
}