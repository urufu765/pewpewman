using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;
using Weth.Cards;

namespace Weth.Actions;

public static class SplitshotTranspiler
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(IgnoreMissingDroneCheck))
        );
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(IgnoreDroneBloops))
        );
        harmony.Patch(
            original: typeof(AAttack).GetMethod("Begin", AccessTools.all),
            transpiler: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(DontDoDuplicateArtifactModifiers))
        );
        harmony.Patch(
            original: typeof(Card).GetMethod("RenderAction", AccessTools.all),
            prefix: new HarmonyMethod(typeof(SplitshotTranspiler), nameof(IconRenderingStuff))
        );
    }

    private static IEnumerable<CodeInstruction> DontDoDuplicateArtifactModifiers(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.WholeSequence,
                ILMatches.Ldarg(2),
                ILMatches.AnyCall,
                ILMatches.AnyCall,
                ILMatches.AnyStloc)
                .Find(SequenceBlockMatcherFindOccurence.Last,
                SequenceMatcherRelativeBounds.BeforeOrEnclosed,
                ILMatches.Ldarg(0),
                ILMatches.Ldflda(AccessTools.DeclaredField(typeof(AAttack), "fromDroneX")),
                ILMatches.AnyCall,
                ILMatches.Brtrue.GetBranchTarget(out var target))
                .Insert(SequenceMatcherPastBoundsDirection.After,
                SequenceMatcherInsertionResultingBounds.JustInsertion,
                [
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SplitshotTranspiler), nameof(IsSplit))),
                    new(OpCodes.Brtrue_S, target.Value)
                ]).AllElements();
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "HGDBDGEGIOEWRHFGUIOHAWEF");
            throw;
        }
    }

    private static bool IconRenderingStuff(G g, State state, CardAction action, bool dontDraw, int shardAvailable, int stunChargeAvailable, int bubbleJuiceAvailable, ref int __result)
    {
        if (action is not ASplitshot splitshot)
        {
            return true;
        }

        var copy = ASplitshot.ConvertSplitToAttack(splitshot, true);
        var position = g.Push(rect: new()).rect.xy;
        int initialX = (int)position.x;

        position.x += Card.RenderAction(g, state, copy, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable);
        g.Pop();


        __result = (int)position.x - initialX;

        return false;

    }

    // private static IEnumerable<CodeInstruction> IgnoreMissingDroneCheck(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    // {
    //     try
    //     {
    //         return new SequenceBlockMatcher<CodeInstruction>(instructions)
    //             .Find(SequenceBlockMatcherFindOccurence.First,
    //             SequenceMatcherRelativeBounds.Enclosed,
    //             ILMatches.AnyCall,
    //             ILMatches.AnyCall,
    //             ILMatches.Brtrue.GetBranchTarget(out var target))
    //             .Insert(SequenceMatcherPastBoundsDirection.After,
    //             SequenceMatcherInsertionResultingBounds.JustInsertion,
    //             [
    //                 new(OpCodes.Ldarg_0),
    //                 new(OpCodes.Ldflda, AccessTools.DeclaredField(typeof(AAttack), "fromX")),
    //                 new(OpCodes.Ldc_I4_S, -1984),
    //                 new(OpCodes.Beq_S, target.Value)
    //             ]
    //             ).AllElements();
    //     }
    //     catch (Exception err)
    //     {
    //         Console.WriteLine("FUCK, IL shit went FUCKITY");
    //         Console.WriteLine(err);
    //         throw;
    //     }
    // }


    /// <summary>
    /// Allows splitshots to function similarily as an AAttack
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="il"></param>
    /// <returns></returns>
    private static IEnumerable<CodeInstruction> IgnoreMissingDroneCheck(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.Enclosed,
                ILMatches.AnyCall,
                ILMatches.AnyCall,
                ILMatches.Brtrue.GetBranchTarget(out var target))
                .Insert(SequenceMatcherPastBoundsDirection.After,
                SequenceMatcherInsertionResultingBounds.JustInsertion,
                [
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SplitshotTranspiler), nameof(IsSplit))),
                    new(OpCodes.Brtrue_S, target.Value)
                ]).AllElements();
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "FUCK, IL shit went FUCKITY");
            throw;
        }
    }


    /// <summary>
    /// Prevents splitshots from triggering drones VFX.
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="il"></param>
    /// <returns></returns>
    private static IEnumerable<CodeInstruction> IgnoreDroneBloops(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.Last,
                SequenceMatcherRelativeBounds.Enclosed,
                ILMatches.Ldflda(AccessTools.DeclaredField(typeof(AAttack), "fromDroneX")),
                ILMatches.AnyCall,
                ILMatches.Brfalse.GetBranchTarget(out var target)
                ).Insert(SequenceMatcherPastBoundsDirection.After,
                SequenceMatcherInsertionResultingBounds.JustInsertion,
                [
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SplitshotTranspiler), nameof(IsSplit))),
                    new(OpCodes.Brtrue_S, target.Value)
                ]).AllElements();
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "FOCKIN IL BORK");
            throw;
        }
    }


    private static bool IsSplit(AAttack attack)
    {
        return ModEntry.Instance.Helper.ModData.TryGetModData<bool>(attack, "split", out var b) && b;
    }
}

public class ASplitshot : CardAction
{
    public int damage;
    public int? givesEnergy;
    public Status? status;
    public int statusAmount;
    public Card? cardOnHit;
    public CardDestination destination;
    public bool stunEnemy;
    public int moveEnemy;
    public bool weaken;
    public bool brittle;
    public bool armorise;
    public bool piercing;
    public bool targetPlayer;
    public bool fast;
    public bool paybackAttack;
    public bool multiCannonVolley;
    public int paybackCounter;
    public bool storyFromStrafe;
    public bool storyFromPayback;
    public List<CardAction>? onKillActions;
    public int? fromX;



    public override void Begin(G g, State s, Combat c)
    {
        Ship toShip = targetPlayer ? s.ship : c.otherShip;
        Ship fromShip = targetPlayer ? c.otherShip : s.ship;
        if (toShip is null || fromShip is null)
        {
            return;
        }
        if (toShip.hull <= 0)
        {
            return;
        }
        int? n = GetFromX(s, c);
        RaycastResult? raycastResult = n is not null ? CombatUtils.RaycastFromShipLocal(s, c, n.Value, targetPlayer) : null;
        //bool librahit = fromShip.Get(Status.libra) > 0;

        // Checks if attack is multicannon then separates them
        if (!targetPlayer && g.state.ship.GetPartTypeCount(PType.cannon, false) > 1 && !multiCannonVolley)
        {
            c.QueueImmediate(new AVolleySplitshotFromAllCannons
            {
                splitshot = Mutil.DeepCopy(this)
            });
            timer = 0.0;
            return;
        }

        // Artifact modifiers
        if (!targetPlayer)
        {
            foreach (Artifact artifact in s.EnumerateAllArtifacts())
            {
                bool? toPierce = artifact.OnPlayerAttackMakeItPierce(s, c);
                if (toPierce is not null && toPierce.Value)
                {
                    piercing = true;
                    artifact.Pulse();
                }
                bool? tostun = artifact.ModifyAttacksToStun(s, c);
                if (tostun is not null && tostun.Value)
                {
                    stunEnemy = true;
                    artifact.Pulse();
                }
            }
        }

        if (!stunEnemy && s.ship.Get(Status.stunCharge) > 0 && !targetPlayer)
        {
            s.ship.Set(Status.stunCharge, s.ship.Get(Status.stunCharge) - 1);
            stunEnemy = true;
        }

        if (raycastResult is not null && raycastResult.hitDrone)
        {
            bool invincible = c.stuff[raycastResult.worldX].Invincible();
            foreach (Artifact artifact in s.EnumerateAllArtifacts())
            {
                bool? droneInvincibilityModified = artifact.ModifyDroneInvincibility(s, c, c.stuff[raycastResult.worldX]);
                if (droneInvincibilityModified is not null && droneInvincibilityModified.Value)
                {
                    invincible = true;
                    artifact.Pulse();
                }
            }
            if (c.stuff[raycastResult.worldX].bubbleShield)
            {
                c.stuff[raycastResult.worldX].bubbleShield = false;
            }
            else if (invincible)
            {
                c.QueueImmediate(c.stuff[raycastResult.worldX].GetActionsOnShotWhileInvincible(s, c, !targetPlayer, damage));
            }
            else
            {
                c.DestroyDroneAt(s, raycastResult.worldX, !targetPlayer);
            }
            if (!piercing)
            {
                ModEntry.Instance.Logger.LogInformation("Split!");
                AAttack left = ConvertSplitToAttack(this);
                AAttack right = ConvertSplitToAttack(this);
                ModEntry.Instance.Helper.ModData.SetModData(left, "split", true);
                ModEntry.Instance.Helper.ModData.SetModData(right, "split", true);
                left.fast = right.fast = true;
                left.fromDroneX = raycastResult.worldX - 1;
                right.fromDroneX = raycastResult.worldX + 1;
                c.QueueImmediate([left, right]);
                timer = 0.0;
                return;
            }
        }
        ModEntry.Instance.Helper.ModData.SetModData(this, "split", true);
        c.QueueImmediate(ConvertSplitToAttack(this));
        timer = 0.0;
    }



    public static AAttack ConvertSplitToAttack(ASplitshot splitshot, bool fake=false)
    {
        if (fake)
        {
            return new APseudoAttack
            {
                damage = splitshot.damage,
                givesEnergy = splitshot.givesEnergy,
                status = splitshot.status,
                statusAmount = splitshot.statusAmount,
                cardOnHit = splitshot.cardOnHit,
                destination = splitshot.destination,
                moveEnemy = splitshot.moveEnemy,
                stunEnemy = splitshot.stunEnemy,
                weaken = splitshot.weaken,
                brittle = splitshot.brittle,
                armorize = splitshot.armorise,
                piercing = splitshot.piercing,
                targetPlayer = splitshot.targetPlayer,
                fast = splitshot.fast,
                paybackAttack = splitshot.paybackAttack,
                multiCannonVolley = splitshot.multiCannonVolley,
                paybackCounter = splitshot.paybackCounter,
                storyFromStrafe = splitshot.storyFromStrafe,
                storyFromPayback = splitshot.storyFromPayback,
                onKillActions = splitshot.onKillActions,
                fromX = splitshot.fromX,
            };
        }
        return new AAttack
        {
            damage = splitshot.damage,
            givesEnergy = splitshot.givesEnergy,
            status = splitshot.status,
            statusAmount = splitshot.statusAmount,
            cardOnHit = splitshot.cardOnHit,
            destination = splitshot.destination,
            moveEnemy = splitshot.moveEnemy,
            stunEnemy = splitshot.stunEnemy,
            weaken = splitshot.weaken,
            brittle = splitshot.brittle,
            armorize = splitshot.armorise,
            piercing = splitshot.piercing,
            targetPlayer = splitshot.targetPlayer,
            fast = splitshot.fast,
            paybackAttack = splitshot.paybackAttack,
            multiCannonVolley = splitshot.multiCannonVolley,
            paybackCounter = splitshot.paybackCounter,
            storyFromStrafe = splitshot.storyFromStrafe,
            storyFromPayback = splitshot.storyFromPayback,
            onKillActions = splitshot.onKillActions,
            fromX = splitshot.fromX,
        };
    }


    /// <summary>
    /// May need a rewrite
    /// </summary>
    /// <param name="c"></param>
    /// <param name="target"></param>
    /// <param name="ray"></param>
    /// <returns></returns>
    private bool ApplyAutododge(Combat c, Ship target, RaycastResult ray)
    {
        if (ray.hitShip)
        {
            if (target.Get(Status.autododgeRight) > 0)
            {
                target.Add(Status.autododgeRight, -1);
                int n = ray.worldX - target.x + 1;
                c.QueueImmediate([
                    new AMove
                    {
                        targetPlayer = this.targetPlayer,
                        dir = n,
                    },
                    this
                ]);
                this.timer = 0.0;
                return true;
            }
            if (target.Get(Status.autododgeLeft) > 0)
            {
                target.Add(Status.autododgeLeft, -1);
                int n = ray.worldX - target.x - 1;
                c.QueueImmediate([
                    new AMove
                    {
                        targetPlayer = this.targetPlayer,
                        dir = n,
                    },
                    this
                ]);
                this.timer = 0.0;
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Adapted from AAttack
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = new List<Tooltip>();
        Combat? combat = (Combat)s.route;
        int n = s.ship.x;
        foreach (Part part in s.ship.parts)
        {
            if (part.type == PType.cannon && part.active)
            {
                if (combat is not null && combat.stuff.ContainsKey(n))
                {
                    combat.stuff[n].hilight = 2;
                }
                part.hilight = true;
            }
            n++;
        }
        if (combat is not null)
        {
            foreach (StuffBase stuff in combat.stuff.Values.Where(stuff => stuff is JupiterDrone))
            {
                stuff.hilight = 2;
            }
        }
        if (piercing)
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.piercingsplitshot")
            {
                Icon = ModEntry.Instance.SprSplitshotPiercing,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Splitshot", "name"]),
                TitleColor = Colors.action,
                Description = ModEntry.Instance.Localizations.Localize(["action", "Splitshot", "pierceDesc"])

            });
        }
        else
        {
            tooltips.Add(new GlossaryTooltip("actiontooltip.splitshot")
            {
                Icon = ModEntry.Instance.SprSplitshot,
                Title = ModEntry.Instance.Localizations.Localize(["action", "Splitshot", "name"]),
                TitleColor = Colors.action,
                Description = ModEntry.Instance.Localizations.Localize(["action", "Splitshot", "desc"])

            });
        }
        if (status is not null)
        {
            tooltips.AddRange(StatusMeta.GetTooltips(status.Value, statusAmount));
        }
        if (stunEnemy || s.ship.Get(Status.stunCharge) > 0)
        {
            tooltips.Add(new TTGlossary("action.stun"));
        }
        if (weaken)
        {
            tooltips.Add(new TTGlossary("parttrait.weak"));
        }
        if (brittle)
        {
            tooltips.Add(new TTGlossary("parttrait.brittle"));
        }
        if (armorise)
        {
            tooltips.Add(new TTGlossary("parttrait.armor"));
        }
        if (moveEnemy < 0)
        {
            tooltips.Add(new TTGlossary("action.moveLeftEnemy", [Math.Abs(moveEnemy)]));
        }
        if (moveEnemy > 0)
        {
            tooltips.Add(new TTGlossary("action.moveRightEnemy", [moveEnemy]));
        }
        if (s.route is Combat && !DoWeHaveCannonsThough(s))
        {
            tooltips.Add(new TTGlossary("action.attackFailWarning.desc"));
        }
        return tooltips;
    }


    /// <summary>
    /// Adapted from AAttack
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public override Icon? GetIcon(State s)
    {
        if (DoWeHaveCannonsThough(s))
        {
            return new Icon(this.piercing? ModEntry.Instance.SprSplitshotPiercing : ModEntry.Instance.SprSplitshot, damage, Colors.redd, false);
        }
        return new Icon(this.piercing? ModEntry.Instance.SprSplitshotPiercingFail : ModEntry.Instance.SprSplitshotFail, damage, Colors.attackFail, false);
    }


    /// <summary>
    /// Copy from AAttack
    /// </summary>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    private int? GetFromX(State s, Combat c)
    {
        if (this.fromX is not null)
        {
            return this.fromX;
        }
        int n = (targetPlayer ? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.cannon && p.active);
        if (n != -1)
        {
            return n;
        }
        return null;
    }


    /// <summary>
    /// Copy from AAttack
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private bool DoWeHaveCannonsThough(State s)
    {
        if (s.ship.parts.Any(part => part.type == PType.cannon))
        {
            return true;
        }
        Combat combat = (Combat)s.route;
        if (combat is not null && combat.stuff.Values.Any(stuff => stuff is JupiterDrone))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Copy from AAttack
    /// </summary>
    /// <param name="c"></param>
    /// <param name="source"></param>
    private void DoLibraEffect(Combat c, Ship source)
    {
        c.QueueImmediate(new AStatus
        {
            targetPlayer = !this.targetPlayer,
            status = Status.tempShield,
            statusAmount = source.Get(Status.libra)
        });
    }
}