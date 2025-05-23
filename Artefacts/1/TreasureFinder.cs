using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ])]
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

    public override bool CanBeDepleted()
    {
        return false;
    }

    public override Upgrade GetUpgrade()
    {
        return Upgrade.A;
    }

    public override void OnReceiveArtifact(State state)
    {
        UhDuhHundo.ArtifactRemover(ref state, typeof(TreasureHunter).Name);
    }
}