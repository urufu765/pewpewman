using System.Collections.Generic;
using System.Linq;
using FMOD;
using FSPRO;
using Nickel;

namespace Weth.Actions;

public class ABayBlast : CardAction
{
    public bool wide;
    public bool bubbleLeech;
    public bool harmless;
    public bool targetPlayer;
    public bool multiBayVolley;
    public bool fast;
    public int? fromX;
    public int? worldX;


    public override void Begin(G g, State s, Combat c)
    {
        Ship toShip = targetPlayer? s.ship : c.otherShip;
        Ship fromShip = targetPlayer? c.otherShip : s.ship;
        if (toShip is null || fromShip is null) return;
        if (toShip.hull <= 0) return;
        int? n = GetFromX(s, c);
        RaycastResult? raycastResult = worldX != null? CombatUtils.RaycastGlobal(c, toShip, false, worldX.Value) : (n != null? CombatUtils.RaycastFromShipLocal(s, c, n.Value, targetPlayer) : null);
        if (!targetPlayer && g.state.ship.GetPartTypeCount(PType.missiles, false) > 1 && !multiBayVolley)
        {
            c.QueueImmediate(new AVolleyBlastFromAllBays
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
                        targetPlayer = !targetPlayer
                    });
                }
            }
            if (invincible)
            {
                Audio.Play(new GUID?(Event.Hits_HitDrone));
                c.QueueImmediate(c.stuff[raycastResult.worldX].GetActionsOnShotWhileInvincible(s, c, !targetPlayer, harmless? 0 : Card.GetActualDamage(s, 1, targetPlayer)));
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
                c.DestroyDroneAt(s, raycastResult.worldX, targetPlayer);
            }
        }
        this.timer = fast? 0.2 : 0.4;
        if (wide)
        {
            c.QueueImmediate([
                new ABayBlast
                {
                    worldX = raycastResult != null? raycastResult.worldX - 1 : FromLocalToWorld(s, c, n, targetPlayer, -1),
                    bubbleLeech = this.bubbleLeech,
                    targetPlayer = this.targetPlayer,
                    harmless = this.harmless,
                    wide = false,
                    fast = true,
                    multiBayVolley = false
                },
                new ABayBlast
                {
                    worldX = raycastResult != null? raycastResult.worldX + 1 : FromLocalToWorld(s, c, n, targetPlayer, 1),
                    bubbleLeech = this.bubbleLeech,
                    targetPlayer = this.targetPlayer,
                    harmless = this.harmless,
                    wide = false,
                    fast = true,
                    multiBayVolley = false
                }
            ]);
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
            return new Icon(this.wide? ModEntry.Instance.SprBayBlastWide : ModEntry.Instance.SprBayBlast, null, Colors.attack, false);
        }
        return new Icon(this.wide? ModEntry.Instance.SprBayBlastWideFail : ModEntry.Instance.SprBayBlastFail, null, Colors.attackFail, false);
    }

    private int? GetFromX(State s, Combat c)
    {
        if (fromX != null) return fromX;
        int n = (targetPlayer? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.missiles && p.active);
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
        if (wide)
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.widebayblast")
            {
                Icon = ModEntry.Instance.SprBayBlastWide,
                Title = ModEntry.Instance.Localizations.Localize(["action", "WideBayblast", "name"]),
                TitleColor = Colors.action,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "WideBayblast", "desc"]), $"<c=boldPink>{Card.GetActualDamage(s, 1, targetPlayer)}</c>")
            });
        }
        else 
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.bayblast")
            {
                Icon = ModEntry.Instance.SprBayBlast,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Bayblast", "name"]),
                TitleColor = Colors.action,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "Bayblast", "desc"]), $"<c=boldPink>{Card.GetActualDamage(s, 1, targetPlayer)}</c>")
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

    private static bool HaveWeGotAnyMissileBays(State s)
    {
        if (s.ship.parts.Any(part => part.type == PType.missiles))
        {
            return true;
        }
        return false;
    }
};