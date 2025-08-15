using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OneOf.Types;
using Weth.Actions;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class HeraldNihility : WethRelicFour
{
    public override int? GetDisplayNumber(State s)
    {
        return null;
    }


    public override void OnCombatEnd(State state)
    {
        List<Upgrade> possibleUpgrades = [.. (
            from card in state.deck
            where card.GetMeta().upgradesTo.Length > 0
            select card.upgrade
        ).Distinct()];

        if (possibleUpgrades.Count == 0)
        {
            return;  // When you literally have no upgradable cards?!
        }
        state.rewardsQueue.QueueImmediate(new ACardSelect
        {
            browseAction = new AWethSelectSingleCardCycleUpgrade(),
            browseSource = CardBrowse.Source.Deck,
            filterUpgrade = possibleUpgrades.Random(state.rngActions),
            filterAvailableUpgrade = Upgrade.A,  // Cards that at least have upgrade A will have an upgrade path.
            allowCloseOverride = Special,
            artifactPulse = Key()
        });
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary(Special ? "action.upgradeCard" : "action.upgradeCardRandom"));
        return tips;
    }
}

[ArtifactMeta(pools = [ArtifactPool.Unreleased, ArtifactPool.Boss])]
public class HeraldNihilityFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(HeraldNihility);

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary(Special? "action.upgradeCard" : "action.upgradeCardRandom"));
        return tips;
    }
}