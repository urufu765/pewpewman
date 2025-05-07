using System.Collections.Generic;
using System.Linq;
using FMOD;
using FSPRO;
using HarmonyLib;
using Nickel;

namespace Weth.Actions;

// public static class BayBlastIconography
// {
//     public static void Apply(Harmony harmony)
//     {
//         harmony.Patch(
//             original: typeof(Card).GetMethod("RenderAction", AccessTools.all),
//             prefix: new HarmonyMethod(typeof(BayBlastIconography), nameof(IconRenderingStufff))
//         );
//     }

//     private static bool IconRenderingStufff(G g, State state, CardAction action, bool dontDraw, int shardAvailable, int stunChargeAvailable, int bubbleJuiceAvailable, ref int __result)
//     {
//         if (action is not ABayBlastV2 bayblast)
//         {
//             return true;
//         }

//         var copy = ABayBlastV2.GetFakeAttackFromBayBlast(bayblast);
//         var position = g.Push(rect: new()).rect.xy;
//         int initialX = (int)position.x;

//         position.x += Card.RenderAction(g, state, copy, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable);
//         g.Pop();


//         __result = (int)position.x - initialX;

//         return false;

//     }

// }

public class ABayBlastV2 : CardAction
{
    public bool flared;
    public bool bubbleLeech = true;
    public bool harmless;
    public bool fromPlayer = true;
    public bool TargetPlayer => flared? fromPlayer:!fromPlayer;
    public bool multiBayVolley;
    public int range = 1;
    public bool fast;
    public int? fromX;
    public int? worldX;

    public static AAttack GetFakeAttackFromBayBlast(ABayBlastV2 bayblast)
    {
        return new APseudoBayblast
        {
            flared = bayblast.flared,
            damage = bayblast.range,
            targetPlayer = bayblast.TargetPlayer,
            xHint = bayblast.xHint,
            whoDidThis = bayblast.whoDidThis,
            artifactPulse = bayblast.artifactPulse,
            statusPulse = bayblast.statusPulse,
            omitFromTooltips = bayblast.omitFromTooltips,
            dialogueSelector = bayblast.dialogueSelector,
            disabled = bayblast.disabled,
            selectedCard = bayblast.selectedCard,
            canRunAfterKill = bayblast.canRunAfterKill,
            shardcost = bayblast.shardcost
        };
    }

    public override void Begin(G g, State s, Combat c)
    {
        Ship fromShip = fromPlayer? s.ship : c.otherShip;
        Ship toShip = fromPlayer? c.otherShip : s.ship;
        if (toShip is null || fromShip is null) return;
        if (toShip.hull <= 0) return;
        int? n = GetFromX(s, c);
        RaycastResult? raycastResult = worldX is int wx? CombatUtils.RaycastGlobal(c, toShip, false, wx) : (n is int nn? CombatUtils.RaycastFromShipLocal(s, c, nn, !fromPlayer) : null);
        if (fromPlayer && g.state.ship.GetPartTypeCount(PType.missiles, false) > 1 && !multiBayVolley)
        {
            c.QueueImmediate(new AVolleyBlastFromAllBays2
            {
                bayblast = Mutil.DeepCopy(this)
            });
            this.timer = 0.0;
            return;
        }
        if (raycastResult is null)
        {
            timer = 0.0;
            return;
        }
        if (!raycastResult.hitDrone)
        {
            Audio.Play(new GUID?(Event.Hits_HitDrone));
            c.fx.Add(new AsteroidExplosion 
            {
                pos = new Vec(raycastResult.worldX * 16, 60.0) + new Vec(7.5, 4.0)
            });
        }
        if (raycastResult is not null && raycastResult.hitDrone)
        {
            bool invincible = c.stuff[raycastResult.worldX].Invincible();
            foreach (Artifact artifact in s.EnumerateAllArtifacts())
            {
                bool? dronInvincibilityModified = artifact.ModifyDroneInvincibility(s, c, c.stuff[raycastResult.worldX]);
                if (dronInvincibilityModified is not null && dronInvincibilityModified.Value)
                {
                    invincible = true;
                    artifact.Pulse();
                }
            }
            if (c.stuff[raycastResult.worldX].bubbleShield)
            {
                c.stuff[raycastResult.worldX].bubbleShield = false;
                if (bubbleLeech)
                {
                    c.QueueImmediate(new AStatus
                    {
                        status = Status.bubbleJuice,
                        statusAmount = 1,
                        targetPlayer = fromPlayer
                    });
                }
            }
            if (invincible)
            {
                Audio.Play(new GUID?(Event.Hits_HitDrone));
                c.QueueImmediate(c.stuff[raycastResult.worldX].GetActionsOnShotWhileInvincible(s, c, TargetPlayer, harmless? 0 : Card.GetActualDamage(s, 1, !fromPlayer)));
            }
            else if (harmless)
            {
                Audio.Play(new GUID?(Event.Hits_BeamHit));
                c.fx.Add(new AsteroidExplosion 
                {
                    pos = new Vec(raycastResult.worldX * 16, 60.0) + new Vec(7.5, 4.0)
                });
                List<CardAction>? actions = c.stuff[raycastResult.worldX]?.GetActions(s, c);
                if (actions is not null)
                {
                    c.Queue(actions);
                }
                c.Queue(new ADummyAction
                {
                    timer = 0.0
                });
            }
            else
            {
                Audio.Play(new GUID?(Event.Hits_HitDrone));
                c.DestroyDroneAt(s, raycastResult.worldX, !TargetPlayer);
            }
        }
        this.timer = fast? 0.2 : 0.4;
        if (range > 1)
        {
            List<CardAction> actions = [];
            for (int i = 1; i < range; i++)
            {
                actions.Add(
                    new ABayBlastV2
                    {
                        worldX = raycastResult != null? raycastResult.worldX - i : FromLocalToWorld(s, c, n, !fromPlayer, -i),
                        bubbleLeech = this.bubbleLeech,
                        fromPlayer = this.fromPlayer,
                        harmless = this.harmless,
                        flared = this.flared,
                        range = 1,
                        fast = true,
                        multiBayVolley = this.multiBayVolley
                    }
                );
                actions.Add(
                    new ABayBlastV2
                    {
                        worldX = raycastResult != null? raycastResult.worldX + i : FromLocalToWorld(s, c, n, !fromPlayer, i),
                        bubbleLeech = this.bubbleLeech,
                        fromPlayer = this.fromPlayer,
                        harmless = this.harmless,
                        flared = this.flared,
                        range = 1,
                        fast = true,
                        multiBayVolley = this.multiBayVolley
                    }
                );
            }
            c.QueueImmediate(actions);
            timer = 0.2;
        }
    }


    private int? FromLocalToWorld(State s, Combat c, int? localX, bool targetPlayer, int offset=0)
    {
        if (localX is null) return null;
        return (!targetPlayer? s.ship : c.otherShip).x + localX + offset;
    }


    public override Icon? GetIcon(State s)
    {
        if (HaveWeGotAnyMissileBays(s))
        {
            return new Icon(this.flared? ModEntry.Instance.SprBayBlastFlared : ModEntry.Instance.SprBayBlastWide, range, Colors.cheevoGold, false);
        }
        return new Icon(ModEntry.Instance.SprBayBlastGeneralFail, range, Colors.attackFail, false);
    }

    private int? GetFromX(State s, Combat c)
    {
        if (fromX != null) return fromX;
        int n = (TargetPlayer? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.missiles && p.active);
        if (n != -1)
        {
            return n;
        }
        return null;
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = new List<Tooltip>();
        Combat? combat = (Combat)s.route;
        for (int x = 0, n = s.ship.x; x < s.ship.parts.Count; x++,n++)
        {
            if (s.ship.parts[x].type == PType.missiles && s.ship.parts[x].active)
            {
                if (combat is not null && combat.stuff.ContainsKey(n))
                {
                    combat.stuff[n].hilight = 2;
                }
                s.ship.parts[x].hilight = true;
                // if (wide && x > 0)
                // {
                //     s.ship.parts[x - 1].hilight = true;
                // }
                // if (wide && x < s.ship.parts.Count - 1)
                // {
                //     s.ship.parts[x + 1].hilight = true;
                // }
            }
        }
        if (flared)
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.flaredbayblast")
            {
                Icon = ModEntry.Instance.SprBayBlastFlared,
                Title = ModEntry.Instance.Localizations.Localize(["action", "FlaredBayblast", "name"]),
                TitleColor = Colors.action,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "FlaredBayblast", "desc"]), $"<c=boldPink>{Card.GetActualDamage(s, 1, !fromPlayer)}</c>", $"<c=boldPink>{this.range}</c>")
            });
        }
        else 
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.bayblasttwo")
            {
                Icon = ModEntry.Instance.SprBayBlastWide,
                Title = ModEntry.Instance.Localizations.Localize(["action", "BayblastV2", "name"]),
                TitleColor = Colors.action,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "BayblastV2", "desc"]), $"<c=boldPink>{Card.GetActualDamage(s, 1, !fromPlayer)}</c>", $"<c=boldPink>{this.range}</c>")
            });
        }
        if (bubbleLeech)
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.bubbleleech")
            {
                Icon = StableSpr.icons_bubbleJuice,
                Title = ModEntry.Instance.Localizations.Localize(["action", "BubbleLeech", "name"]),
                TitleColor = Colors.drone,
                Description = ModEntry.Instance.Localizations.Localize(["action", "BubbleLeech", "desc"])
            });
        }
        if (harmless)
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.harmlessstuff")
            {
                Icon = StableSpr.icons_droneTurn,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Harmless", "name"]),
                TitleColor = Colors.action,
                Description = ModEntry.Instance.Localizations.Localize(["action", "Harmless", "desc"])
            });
        }
        if (s.route is Combat && !HaveWeGotAnyMissileBays(s))
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.bayblastfailure")
            {
                Description = ModEntry.Instance.Localizations.Localize(["action", "BayBlastFail", "desc"])
            });
        }
        return tooltips;
    }

    public static bool HaveWeGotAnyMissileBays(State s)
    {
        if (s.ship.parts.Any(part => part.type == PType.missiles && part.active))
        {
            return true;
        }
        return false;
    }
};