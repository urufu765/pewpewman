namespace Weth.API;

public interface IArtifactWethGoodieUncommonRestrictor
{
    /// <summary>
    /// Sets a restriction of 1 uncommon goodie in the deck at all times.
    /// </summary>
    /// <returns>Whether this artifact will impose a restriction on how many uncommons you can have</returns>
    public bool DoIImposeGoodieUncommonRestriction();

    /// <summary>
    /// Overrides the imposed restriction to allow any amount of goodie in the deck at all times.
    /// </summary>
    /// <returns>Whether to ignore any restrictions placed on the uncommon goodies</returns>
    public bool DoIOverrideGoodieUncommonRestriction();
}