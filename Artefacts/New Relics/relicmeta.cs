using System;

namespace Weth.Artifacts;

public class RelicMeta : Attribute
{
    public WethRelics theRelic;
}

public abstract record RelicData
{
}