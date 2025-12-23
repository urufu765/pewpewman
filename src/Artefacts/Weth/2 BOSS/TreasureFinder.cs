using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Boss])]
public class TreasureSeeker : TreasureHunter
{
    public override bool GetAdvanced()
    {
        return true;
    }

    public override string GetArtifactKey()
    {
        return Key();
    }

    public override void OnReceiveArtifact(State state)
    {
        UhDuhHundo.ArtifactRemover(ref state, typeof(TreasureHunter).Name);
    }
}