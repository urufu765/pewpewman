using System;
using System.Collections.Generic;
using System.Linq;
using Shockah.Kokoro;
using Weth.External;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Unreleased])]
public class ShockStack : WethRelicFour
{
    public List<int> ShockPoints { get; set; } = [];
    public int OldAmount {get;set;}

    // Shock stack visible in HP bar

    public override void OnCombatStart(State state, Combat combat)
    {
        OldAmount = GetAmount();
        ShockPoints = [];
        if (combat.otherShip is not null && combat.otherShip.hullMax > 1)
        {
            ShockPoints = [
                .. Enumerable.Range(0, GetAmount())
                .Select(_ => (int)Math.Floor(state.rngActions.Next() * (combat.otherShip.hullMax - 2)) + 1)
                .Distinct()
            ];
        }

        state.ship.Set(ModEntry.Instance.NewRelicStatuses[GetType()], ShockPoints.Count);
    }

    public override void UpdateStack(State state, bool? special = null, int uncounted = 0)
    {
        if (state.route is Combat c && c.otherShip.hull > 1)
        {
            for (int x = 0; x < GetAmount() + uncounted - OldAmount; x++)
            {
                for (int attempt = 0; attempt < 3; attempt++)
                {
                    int tryShock = (int)Math.Floor(state.rngActions.Next() * (c.otherShip.hull - 2)) + 1;
                    if (!ShockPoints.Contains(tryShock))
                    {
                        ShockPoints.Add(tryShock);
                        break;
                    }
                }
            }
        }
        base.UpdateStack(state, special, uncounted);
    }


    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        if (ShockPoints.Count > 0 && combat.otherShip is not null)
        {
            int i = 0;
            Stack<int> sorted = new(ShockPoints.OrderBy(x => x));

            while (sorted.TryPeek(out int x) && combat.otherShip.hull <= x)
            {
                sorted.Pop();
                i++;
            }

            ShockPoints = [.. sorted];

            if (i > 0)
            {
                combat.QueueImmediate(new AStatus
                {
                    status = Status.stunCharge,
                    statusAmount = i,
                    targetPlayer = true,
                    artifactPulse = Key(),
                    statusPulse = ModEntry.Instance.NewRelicStatuses[GetType()]
                });
            }
        }
    }


    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tips = base.GetExtraTooltips() ?? [];
        tips.Add(new TTGlossary("status.stunCharge", [1]));
        return tips;
    }
}


[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class ShockStackFake : WethRelicFourFake
{
    public override Type RealRelicType => typeof(ShockStack);
}


public class ShockStackStatus : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public ShockStackStatus()
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
        if (args.Status == ModEntry.Instance.NewRelicStatuses[typeof(ShockStack)]) return false;
        return null;
    }

    public IKokoroApi.IV2.IStatusRenderingApi.IStatusInfoRenderer? OverrideStatusInfoRenderer(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusInfoRendererArgs args)
    {
        if (args.Status != ModEntry.Instance.NewRelicStatuses[typeof(ShockStack)]) return null;
        if
        (
            args.State.EnumerateAllArtifacts().Find(a => a is ShockStack) is ShockStack ss
        )
        {
            List<Color> SegmentColours = [];
            int rowCount = args.Amount switch
            {
                <4 => 1,
                <9 => 2,
                _ => 3
            };

            for (int i = 0; i < ss.Amount; i++)
            {
                if (i == ss.ShockPoints.Count - 1 && args.State.route is Combat combat && combat.otherShip.hull - 1 == ss.ShockPoints.Max())
                {
                    SegmentColours.Add(new Color("ffd723"));
                }
                else if (i <= ss.ShockPoints.Count - 1)
                {
                    SegmentColours.Add(new Color("c69610"));
                }
                else if (i < args.Amount)
                {
                    SegmentColours.Add(ModEntry.Instance.KokoroApi.V2.StatusRendering.DefaultInactiveStatusBarColor);
                }
                else
                {
                    SegmentColours.Add(new Color("500e0e"));
                }
            }

            return ModEntry.Instance.KokoroApi.V2.StatusRendering.MakeBarStatusInfoRenderer().SetSegments(SegmentColours).SetSegmentWidth(2).SetRows(rowCount);
        }

        return null;
    }
}
