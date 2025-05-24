using System;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Nickel;

namespace Weth;

/// <summary>
/// Helps out with menial tasks
/// </summary>
public static class UhDuhHundo
{
    public static ArtifactConfiguration ArtifactRegistrationHelper(Type a, Spr sprite, Deck deck)
    {
        ArtifactMeta? attrs = a.GetCustomAttribute<ArtifactMeta>();
        ArtifactPool[] artpl = attrs?.pools ?? new ArtifactPool[1];
        ArtifactConfiguration ac = new ArtifactConfiguration
        {
            ArtifactType = a,
            Meta = new ArtifactMeta
            {
                owner = deck,
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

    public static void ApplySubtleCrystalOverlayGlow(Vec? anchorPoint, (Vec pos, Vec size)[] spots, Color color, double timer, double cycleTime = 4, double minGlow = 0, double maxGlow = 1, bool cascade = false, Vec? extraSize = null)
    {
        try
        {
            for (int i = 0; i < spots.Length; i++)
            {
                Glow.Draw(
                    (anchorPoint ?? new()) + spots[i].pos,
                    extraSize is Vec v? spots[i].size + v : spots[i].size,
                    Color.Lerp(
                        Colors.black,
                        color,
                        Mutil.Lerp(
                            minGlow,
                            maxGlow,
                            (Math.Sin(timer / cycleTime * Math.PI - (cascade && spots.Length > 1? (i * Math.PI / spots.Length - 1) : 0)) + 1) / 2
                        )
                    )
                );
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "Glow thing failed!");
        }
    }

    // public static void ApplyWaveCrystalOverlayGlow(Vec? anchorPoint, (Vec pos, Vec size)[] spots, Color color, double timer, double cycleTime = 4, double minGlow = 0, double maxGlow = 1, double hangTime = 0)
    // {
    //     try
    //     {
    //         for (int i = 0; i < spots.Length; i++)
    //         {
    //             Glow.Draw(
    //                 (anchorPoint ?? new Vec()) + spots[i].pos,
    //                 spots[i].size,
    //                 Color.Lerp(
    //                     Colors.black,
    //                     color,
    //                     Mutil.Lerp(
    //                         minGlow,
    //                         maxGlow,
    //                         (Math.Sin(((timer / cycleTime) + (i / spots.Length)) * Math.PI) + 1) / 4
    //                     )
    //                 )
    //             );
    //         }
    //     }
    //     catch (Exception err)
    //     {
    //         ModEntry.Instance.Logger.LogError(err, "Glow thing failed!");
    //     }
    // }
}

public static class Helpers
{
    public static double InverseLerp(double a, double b, double n)
    {
        if (a == b) return 0;
        return Math.Clamp((n - a) / (b - a), 0, 1);
    }
}

public class TTTTTTText : TTText
{
    public TTTTTTText()
    {
    }

    public TTTTTTText(string text)
    {
        this.text = text;
    }
    public override Rect Render(G g, bool dontDraw)
    {
        return new Rect(0, 0, 0, -10);
    }
}

public class TTTTTTGlossary(string key) : TTGlossary(key)
{
	public Spr? Icon = null;
	public string? Title = null;

	public override Rect Render(G g, bool dontDraw)
	{
		var sb = new StringBuilder();
		if (!string.IsNullOrEmpty(this.Title))
		{
			if (this.Icon is not null)
			{
				sb.Append(GetIndent());
			}

			sb.Append(this.Title);
		}
		var rect = Draw.Text(sb.ToString(), 0, 0, color: Colors.textMain, maxWidth: 100, dontDraw: true);
		if (!dontDraw)
		{
			var xy = g.Push(null, rect).rect.xy;
			if (this.Icon is { } icon)
				Draw.Sprite(icon, xy.x - 1, xy.y + 2);
			Draw.Text(sb.ToString(), xy.x, xy.y + 4, color: Colors.textMain, maxWidth: 100);
			g.Pop();
		}
		return rect;
	}
}