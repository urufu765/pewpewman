using System;
using System.Collections.Generic;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class AstroGrass : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class AstroGrassFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(AstroGrass);
}