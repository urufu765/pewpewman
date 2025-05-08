using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nickel;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;


[ArtifactMeta(pools = [ ArtifactPool.Boss ])]
public class TerminusJaunt : TheTerminus
{
    private const int Goal = 5;
    private Random random = new();
    public (int total, int selected) SavedParts{get; set;} = (0,0);

    public override Spr GetSprite()
    {
        return Mode switch
        {
            Terminus.Active => ModEntry.Instance.SprArtTermJActive,
            Terminus.Inactive => ModEntry.Instance.SprArtTermJInactive,
            Terminus.Reward => ModEntry.Instance.SprArtTermJReward,
            Terminus.AltReward => ModEntry.Instance.SprArtTermJAltReward,
            _ => base.GetSprite()
        };
    }

    public override void OnCombatStart(State state, Combat combat)
    {
        Mode = Terminus.Inactive;
        foreach (int y in Enumerable.Range(0, combat.otherShip.parts.Count).Shuffle(state.rngActions))
        {
            if (combat.otherShip.parts?[y] is not null && combat.otherShip.parts[y].type is not PType.empty)
            {
                ModEntry.Instance.Helper.ModData.SetModData(combat.otherShip.parts[y], "jauntable", true);
                SavedParts = (combat.otherShip.parts.Count - 1, y);
                Mode = Terminus.Active;
                break;
            }
        }
        Pulse();
    }

    public override void OnEnemyGetHit(State state, Combat combat, Part? part)
    {
        if (part is not null && ModEntry.Instance.Helper.ModData.TryGetModData(part, "jauntable", out bool b) && b && Mode == Terminus.Active)
        {
            if (SavedParts != (0, 0))
            {
                ISoundInstance isi = ModEntry.Instance.JauntSlapSound.CreateInstance();
                isi.Volume = 0.45f;
                double lerpVal = Math.Abs(((SavedParts.selected / SavedParts.total) - 0.5) * 2);
                isi.Pitch = (float)Mutil.Lerp(
                    Mutil.Lerp(0.9, 0.75, lerpVal),
                    Mutil.Lerp(1.15, 1, lerpVal),
                    random.NextDouble());
            }
            Counter++;
            if (Counter % Goal == 0)
            {
                Mode = Terminus.AltReward;
            }
            else
            {
                Mode = Terminus.Reward;
            }
            // Custom space bone-breaking sfx
            Pulse();
        }
    }

    public override void OnCombatEnd(State state)
    {
        if (Mode == Terminus.Reward)
        {
            ModEntry.Instance.Logger.LogInformation("Riches GET");
            state.rewardsQueue.Queue(new AArtifactOffering
            {
                amount = 3,
                limitPools = [ArtifactPool.Unreleased],
                limitDeck = ModEntry.Instance.WethDeck.Deck
            });
        }
        if (Mode == Terminus.AltReward)
        {
            ModEntry.Instance.Logger.LogInformation("Bonus Riches GET");
            state.rewardsQueue.Queue(new AArtifactOffering
            {
                amount = 3,
                limitPools = [ArtifactPool.Common]
            });
        }
        Mode = Terminus.Pick;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        return Mode switch
        {
            Terminus.Active => [new GlossaryTooltip("terminusjaunt.active")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "JauntActive"])
            }],
            Terminus.Inactive => [new GlossaryTooltip("terminusjaunt.inactive")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "JauntInactive"])
            }],
            Terminus.Reward => [new GlossaryTooltip("terminusjaunt.reward")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "JauntReward"])
            }],
            Terminus.AltReward => [new GlossaryTooltip("terminusjaunt.altreward")
            {
                Description = ModEntry.Instance.Localizations.Localize(["artifact", "Tooltips", "JauntAltReward"])
            }],
            _ => base.GetExtraTooltips()
        };
    }
}