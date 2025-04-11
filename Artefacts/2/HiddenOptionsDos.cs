using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class HiddenOptions2 : HiddenOptions
{
    public override int GetArtifactAmount()
    {
        return 5;
    }
}