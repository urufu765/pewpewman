using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf.Types;
using Weth.Actions;
using Weth.Cards;


namespace Weth.Artifacts;

[ArtifactMeta(pools = [ ArtifactPool.Common ]), DuoArtifactMeta(duoDeck = Deck.shard)]
public class PowerCrystals : Artifact
{
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if (fromPlayer)
        {
            return state.ship.Get(Status.shard);
        }
        return 0;
    }


    public override void OnTurnStart(State state, Combat combat)
    {
        if (state.ship.Get(Status.shard) > 0)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.shard,
                statusAmount = -1,
                targetPlayer = true,
                artifactPulse = Key(),
            });
        }
    }

    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if(status == ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive && mode == AStatusMode.Add)
        {
            combat.QueueImmediate(
                new AStatus{
                    status = Status.shard,
                    statusAmount = statusAmount,
                    targetPlayer = true,
                    artifactPulse = Key()
                }
            );
            state.ship.Set(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 0);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        int maxAmount = (MG.inst.g?.state ?? DB.fakeState).ship.Get(Status.maxShard);
        if (maxAmount == 0) maxAmount = 3;
        List<Tooltip> l = StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.V2.DriveStatus.Pulsedrive, 1);
        l.Insert(0, new TTGlossary("status.shard", [maxAmount]));
        return l;
    }
}