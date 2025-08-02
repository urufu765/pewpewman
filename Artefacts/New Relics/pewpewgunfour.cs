using System;
using System.Collections.Generic;
using Weth.External;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Common])]
public class PewPewGun : WethRelicFour
{
    public int ShotsLeft { get; set; }

    public override void OnCombatStart(State state, Combat combat)
    {
        base.OnCombatStart(state, combat);
        ShotsLeft = Amount;
    }

    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (ShotsLeft > 0 && deck == ModEntry.Instance.WethDeck.Deck)
        {
            combat.Queue(new AAttack
            {
                damage = card.GetDmg(state, 0),
                fast = true,
                artifactPulse = Key(),
                statusPulse = ModEntry.Instance.NewRelicStatuses[GetType()]
            });
            ShotsLeft--;
        }
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class PewPewGunFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(PewPewGun);
}

public class PewPewGunStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public PewPewGunStatus()
    {
        ModEntry.Instance.KokoroApi.V2.StatusLogic.RegisterHook(this);
        ModEntry.Instance.KokoroApi.V2.StatusRendering.RegisterHook(this);
    }
    
    /// <summary>
    /// No relic statuses should be affected by timestop nor boost
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public bool? IsAffectedByBoost(IKokoroApi.IV2.IStatusLogicApi.IHook.IIsAffectedByBoostArgs args)
    {
        if (args.Status == ModEntry.Instance.NewRelicStatuses[typeof(PewPewGun)]) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.NewRelicStatuses[typeof(PewPewGun)]) return null;
        if
        (
            args.State.EnumerateAllArtifacts().Find(a => a is PewPewGun) is PewPewGun ppg
        )
        {
            bool ranOut = ppg.ShotsLeft <= 0;
            string shotColor = ranOut ? "" : "<c=d38018>";
            string shotColorEnd = ranOut ? "" : "</c>";

            return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeTextStatusInfoRenderer($"{shotColor}{ppg.ShotsLeft}{shotColorEnd}<c=6c8191>|{args.Amount}</c>").SetColor(ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor);
        }

        return null;
    }
}
