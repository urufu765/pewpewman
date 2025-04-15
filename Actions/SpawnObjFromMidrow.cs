using System.Collections.Generic;
using System.Linq;
using FMOD;
using FSPRO;
using Nickel;

namespace Weth.Actions;

public enum Coutcome
{
    BothDie,
    NewbieWins,
    EliteWins
}
public class ASpawnFromMidrow : CardAction
{
    public int worldX;
    public int offset;
    public bool byPlayer;
    public StuffBase thing = null!;


    public override void Begin(G g, State s, Combat c)
    {
        // foreach (Artifact artifact in s.EnumerateAllArtifacts())
        // {
        //     thing = artifact.ReplaceSpawnedThing(s, c, thing, )
        // }
        int spawnX = worldX + offset;
        StuffBase? existingThing;
        bool newbieDies = false;
        bool existingDies = false;
        if (c.stuff.TryGetValue(spawnX, out existingThing))
        {
            Coutcome collisionOutcome = GetCollisionOutcome(thing, existingThing);
            newbieDies = collisionOutcome is Coutcome.BothDie or Coutcome.EliteWins;
            existingDies = collisionOutcome is Coutcome.BothDie or Coutcome.NewbieWins;
            thing.bubbleShield = false;
            existingThing.bubbleShield = false;
        }
        else
        {
            int? leftwallX = c.leftWall;
            int? rightwallX = c.rightWall;
            if (leftwallX is not null && spawnX < leftwallX || rightwallX is not null && spawnX > rightwallX)
            {
                newbieDies = true;
            }
        }
        if (existingThing is not null)
        {
            if (existingDies)
            {
                c.DestroyDroneAt(s, spawnX, byPlayer);
            }
            else if (existingThing.Invincible())
            {
                c.QueueImmediate(existingThing.GetActionsOnBonkedWhileInvincible(s, c, byPlayer, thing));
            }
        }
        if (!newbieDies && !existingDies)
        {
            Audio.Play(new GUID?(Event.Drones_MissileLaunch), true);
        }
        if (newbieDies || existingDies)
        {
            Audio.Play(new GUID?(Event.Hits_DroneCollision), true);
        }
        thing.xLerped = worldX;
        thing.x = spawnX;
        //thing.yAnimation = 0;
        if (existingThing is not null && newbieDies && !existingDies)
        {
            c.stuff[spawnX] = thing;
            c.DestroyDroneAt(s, spawnX, byPlayer);
            c.stuff[spawnX] = existingThing;
            return;
        }
        c.stuff[spawnX] = thing;
        if (newbieDies)
        {
            c.DestroyDroneAt(s, spawnX, byPlayer);
        }
    }

    private static Coutcome GetCollisionOutcome(StuffBase newbie, StuffBase elite)
    {
        if (newbie.Invincible())
        {
            return Coutcome.NewbieWins;
        }
        else if (elite.Invincible())
        {
            return Coutcome.EliteWins;
        }
        else if (newbie.bubbleShield != elite.bubbleShield)
        {
            return newbie.bubbleShield? Coutcome.NewbieWins : Coutcome.EliteWins;
        }
        else
        {
            return Coutcome.BothDie;
        }
    }
}