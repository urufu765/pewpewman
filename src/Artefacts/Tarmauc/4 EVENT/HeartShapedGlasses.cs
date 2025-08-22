using System.Collections.Generic;
using System.Linq;

namespace Weth.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.EventOnly])]
public class HeartShapedGlasses : Artifact
{
    public bool CheapAvailable { get; set; }
    public const int EXACT_COST = 1;

    public override void OnCombatStart(State state, Combat combat)
    {
        if (state.map?.markers?.TryGetValue(state.map.currentLocation, out Marker? m) is true && m?.contents is MapBattle mb && mb.battleType == BattleType.Boss)
        {
            CheapAvailable = true;
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn > 1) CheapAvailable = false;
    }

    public override void OnDrawCard(State state, Combat combat, int count)
    {
        if (CheapAvailable)
        {
            List<Card> possibleCards = [.. combat.hand.Where(a => a.GetDataWithOverrides(state).cost == EXACT_COST)];
            if (possibleCards.Count == 0) return;
            possibleCards.Shuffle(state.rngActions).First().discount -= 1;
            Pulse();
            CheapAvailable = false;
        }
    }
}