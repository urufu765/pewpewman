using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;


[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class TerminusMilestone : TheTerminus
{
    public int Stage {get; set;}
    private List<int> Goals {get;} = [3, 6, 10, 14];
    public int LastGoal {get; set;} = 14;
    public int EnemyType {get; set;} = 0;
    private List<int> Points {get;} = [1, 2, 4];  // Normal, Elite, Boss
    private const int BeyondGoals = 2;

    public override void OnReceiveArtifact(State state)
    {
        Counter = 0;
        LastGoal = Goals[^1];
        Stage = 0;
        Mode = Terminus.Active;
    }

    public override Spr GetSprite()
    {
        return Mode switch {
            Terminus.Active => ModEntry.Instance.SprArtTermMileCommon,
            Terminus.Reward => ModEntry.Instance.SprArtTermMileBoss,
            Terminus.AltReward => ModEntry.Instance.SprArtTermMileRelic,
            _ => base.GetSprite()
        };
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        if (state?.map?.markers[state.map.currentLocation]?.contents is MapBattle mb)
        {
            EnemyType = mb.battleType switch
            {
                BattleType.Elite => 1,
                BattleType.Boss => 2,
                _ => 0
            };
        }
    }

    public override void OnCombatEnd(State state)
    {
        ModEntry.Instance.Logger.LogInformation("Money GET");
        List<CardAction> rewards = [];
        DetermineRewards(ref rewards);
        foreach (CardAction ca in rewards)
        {
            state.rewardsQueue.Queue(ca);
        }
    }

    private void DetermineRewards(ref List<CardAction> result, int stop = 0)
    {
        if (stop > 1)
        {
            ModEntry.Instance.Logger.LogInformation("Infinite Recursion Detected");
            return;
        }
        Counter += Points[EnemyType];
        if (GiveCondition(true))
        {
            result.Add(Stage switch
            {
                0 => new AArtifactOffering
                {
                    amount = 2,
                    limitPools = [ArtifactPool.Common]
                },
                1 => new AArtifactOffering
                {
                    amount = 3,
                    limitPools = [ArtifactPool.Common]
                },
                2 => new AArtifactOffering
                {
                    amount = 2,
                    limitPools = [ArtifactPool.Boss]
                },
                3 => new AArtifactOffering
                {
                    amount = 3,
                    limitPools = [ArtifactPool.Boss]
                },
                _ => new AArtifactOffering
                {
                    amount = 3,
                    limitPools = [ArtifactPool.Unreleased],
                    limitDeck = ModEntry.Instance.WethDeck.Deck
                },
            });
            Stage++;
        }
        if (GiveCondition())
        {
            DetermineRewards(ref result, ++stop);
        }
        Mode = Stage switch
        {
            >3 => Terminus.AltReward,
            >1 => Terminus.Reward,
            _ => Terminus.Active
        };
    }

    private bool GiveCondition(bool advanceGoal = false)
    {
        if (Stage < Goals.Count && Counter >= Goals[Stage])
        {
            return true;
        }
        else if ((Counter - LastGoal)/BeyondGoals > 0)
        {
            if (advanceGoal) LastGoal += BeyondGoals;
            return true;
        }
        return false;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = [];
        if (Stage < Goals.Count)
        {
            tips.Add(new GlossaryTooltip("terminusmilestone.start")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "MilestoneStart"])
            });
            for (int x = Stage; x < Goals.Count; x++)
            {
                tips.Add(new GlossaryTooltip("terminusmilestone." + x)
                {
                    Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "Milestone", x.ToString()])
                });
                if (x < Goals.Count - 1) tips.Add(new TTTTTTText(" "));
            }
        }
        else
        {
            tips.Add(new GlossaryTooltip("terminusmilestone.infinite")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "MilestoneEnd"])
            });
        }
        return tips;
    }
}