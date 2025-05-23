using System;
using System.Collections.Generic;
using System.Linq;
using Weth.Artifacts;

namespace Weth.Actions;


/// <summary>
/// Was gonna be used for relics, but I instead just did the same thing using a prefix of ArtifactReward.getOffering
/// </summary>
public class AWethRelicOffering : AArtifactOffering
{
    public AWethRelicOffering()
    {
        limitDeck = ModEntry.Instance.WethDeck.Deck;
        limitPools = [ArtifactPool.Unreleased];
    }

    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        timer = 0.0;
        if (s.EnumerateAllArtifacts().Find(b => b is SR2Focused) is SR2Focused sr2)
        {
            List<Artifact> relics = [];
            IEnumerable<Artifact> standbyRelics = s.EnumerateAllArtifacts().Where(a => a is RelicShield);
            relics = ConjureUpRelicsFromFocused(sr2, [.. standbyRelics]);
            if (!sr2.AtMax && sr2.ObtainedRelics.Count + standbyRelics.Count() < SR2Focused.RELICLIMIT)
            {
                relics.AddRange(ArtifactReward.GetOffering(g.state, amount - relics.Count, limitDeck, limitPools));
            }
            return new ArtifactReward
            {
                artifacts = relics,
                canSkip = canSkip
            };
        }
        return base.BeginWithRoute(g, s, c);
    }

    private static List<Artifact> ConjureUpRelicsFromFocused(SR2Focused spaceRelic, params Artifact[] otherRelics)
    {
        List<Artifact> a = [];
        HashSet<Type> illegals = [.. otherRelics.Select(a => a.GetType())];
        foreach (Status status in spaceRelic.ObtainedRelics)
        {
            if (!illegals.Contains(SR2Focused.RelicDic[status]) && Activator.CreateInstance(SR2Focused.RelicDic[status]) is Artifact artifact)
            {
                a.Add(artifact);
            }
        }
        return a;
    }
}