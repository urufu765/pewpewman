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
using Weth.Objects;

namespace Weth.Actions;

public static class SplitshotTranspiler
{
    /// <summary>
    /// Hijacks the action icon renderer to use a fake version of AAttack that just changes the icon
    /// </summary>
    public static IEnumerable<CodeInstruction> RenderSplitshotAsAttack(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.WholeSequence,
                ILMatches.AnyLdloc,
                ILMatches.Isinst(typeof(AAttack)),
                ILMatches.AnyStloc)
                .Find(SequenceBlockMatcherFindOccurence.Last,
                SequenceMatcherRelativeBounds.BeforeOrEnclosed,
                ILMatches.AnyLdloc.GetLocalIndex(out var loc))
                .Insert(SequenceMatcherPastBoundsDirection.Before,
                SequenceMatcherInsertionResultingBounds.JustInsertion,
                [
                    new(OpCodes.Ldloc_S, loc.Value),
                    new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SplitshotTranspiler), nameof(FakeSplitshotAsAttack))),
                    new(OpCodes.Stloc_S, loc.Value)
                ]).AllElements();
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "damn.");
            throw;
        }
    }

    /// <summary>
    /// Checks if the action is splitshot, then gives the fake AAttack version to render.
    /// </summary>
    public static CardAction? FakeSplitshotAsAttack(CardAction? action)
    {
        if (action is ASplitshot splitshot)
        {
            return ASplitshot.ConvertSplitToAttack(splitshot, true);
        }
        return action;
    }

    /// <summary>
    /// Ignores calls to pierce/stun modifying artifact method hooks if the aattack is part of Splitshot's split shot.
    /// </summary>
    public static IEnumerable<CodeInstruction> DontDoDuplicateArtifactModifiers(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.WholeSequence,  // C_35
                ILMatches.Ldarg(2),  // 262
                ILMatches.AnyCall,  // 263
                ILMatches.AnyCall,  // 264
                ILMatches.AnyStloc)  // 265
                .Find(SequenceBlockMatcherFindOccurence.Last,
                SequenceMatcherRelativeBounds.BeforeOrEnclosed,
                ILMatches.Ldarg(0),  // 253
                ILMatches.Ldflda(AccessTools.DeclaredField(typeof(AAttack), "fromDroneX")),  // 254
                ILMatches.AnyCall,  // 255
                ILMatches.Brtrue.GetBranchTarget(out var target))  // 256
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

    /// <summary>
    /// Renders the double width icons that belong to Giant and Mega asteroids. Also renders the Milk Soda question mark thing
    /// </summary>
    public static bool IconRenderingStuff(G g, State state, CardAction action, bool dontDraw, int shardAvailable, int stunChargeAvailable, int bubbleJuiceAvailable, ref int __result)
    {
        if (action is ASpawn spawn && spawn.thing is GiantAsteroid or MegaAsteroid)
        {
            if (ModEntry.Instance.Helper.ModData.TryGetModData<bool>(action, "issagiantmegagiant", out bool b) && b)
            {
                return true;
            }
            var copy = Mutil.DeepCopy(action);
            ModEntry.Instance.Helper.ModData.SetModData(copy, "issagiantmegagiant", true);
            var position = g.Push(rect: new()).rect.xy;
            int initialX = (int)position.x;

            position.x += Card.RenderAction(g, state, copy, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable);
            g.Pop();


            __result = (int)position.x - initialX;
            __result += 2;
            if (!dontDraw)
            {
                //Draw.Sprite(ModEntry.Instance.SprGiantAsteroidIcon, initialX + __result, position.y);
                Draw.Text(" ", initialX + __result, position.y, dontSubstituteLocFont: true);
            }
            __result += 8;
            return false;
        }

        if (action is APseudoPulsedriveGiver)
        {
            if (ModEntry.Instance.Helper.ModData.TryGetModData<bool>(action, "ispulsedrive", out bool b) && b)
            {
                return true;
            }
            var copy = Mutil.DeepCopy(action);
            ModEntry.Instance.Helper.ModData.SetModData(copy, "ispulsedrive", true);
            var position = g.Push(rect: new()).rect.xy;
            int initialX = (int)position.x;

            position.x += Card.RenderAction(g, state, copy, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable);
            g.Pop();


            __result = (int)position.x - initialX;
            __result += 1;
            if (!dontDraw)
            {
                //Draw.Sprite(StableSpr.icons_questionMark, initialX + __result, position.y, color: Colors.textMain);
                Draw.Sprite(ModEntry.Instance.PulseQuestionMark, initialX + __result, position.y, color: Colors.textMain);
                //Draw.Text("?", initialX + __result, position.y, dontSubstituteLocFont: true, color: Colors.textMain);
            }
            __result += 9;
            return false;
        }
        return true;

    }
    // private static bool IconRenderingStuff(G g, State state, CardAction action, bool dontDraw, int shardAvailable, int stunChargeAvailable, int bubbleJuiceAvailable, ref int __result)
    // {
    //     if (action is not ASplitshot splitshot)
    //     {
    //         return true;
    //     }

    //     var copy = ASplitshot.ConvertSplitToAttack(splitshot, true);
    //     var position = g.Push(rect: new()).rect.xy;
    //     int initialX = (int)position.x;

    //     position.x += Card.RenderAction(g, state, copy, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable);
    //     g.Pop();


    //     __result = (int)position.x - initialX;

    //     return false;

    // }

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
    /// Allows splitshots to function similarily as an AAttack by ignoring some drone null checks
    /// </summary>
    public static IEnumerable<CodeInstruction> IgnoreMissingDroneCheck(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.Enclosed,
                ILMatches.AnyCall,  // 110
                ILMatches.AnyCall,  // 111
                ILMatches.Brtrue.GetBranchTarget(out var target))  // 112
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

    public static IEnumerable<CodeInstruction> FuckYouIllDoWhatIWant(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.First,
                SequenceMatcherRelativeBounds.Enclosed,
                ILMatches.Ldarg(3),
                ILMatches.Newobj(AccessTools.DeclaredConstructor(typeof(AJupiterShoot)))
                ).Insert(SequenceMatcherPastBoundsDirection.Before,
                SequenceMatcherInsertionResultingBounds.JustInsertion,
                [
                    new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(SplitshotTranspiler), nameof(Report)))
                ]).AllElements();
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "FUCK, IL shit went FUCKITY");
            throw;
        }
    }

    public static void FuckYouIllDoWhatIWantAgain(CardAction a)
    {
        ModEntry.Instance.Logger.LogInformation("Current Action: " + a.GetType().Name);
    }

    public static void FlipModDataFromJupiter(AJupiterShoot __instance)
    {
        if (ModEntry.Instance.Helper.ModData.TryGetModData(__instance.attackCopy, "split", out bool b) && b)
        {
            ModEntry.Instance.Helper.ModData.SetModData(__instance.attackCopy, "split", false);
        }
    }

    private static void Report()
    {
        ModEntry.Instance.Logger.LogInformation("Working");
    }

    /// <summary>
    /// Prevents splitshots from triggering drones VFX.
    /// </summary>
    public static IEnumerable<CodeInstruction> IgnoreDroneBloops(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(SequenceBlockMatcherFindOccurence.Last,
                SequenceMatcherRelativeBounds.Enclosed,  // C_338
                ILMatches.Ldflda(AccessTools.DeclaredField(typeof(AAttack), "fromDroneX")),  // 1948
                ILMatches.AnyCall,  // 1949
                ILMatches.Brfalse.GetBranchTarget(out var target)  // 1950
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

    /// <summary>
    /// Checks if the AAttack is a split shot from a ASplitshot
    /// </summary>
    /// <param name="attack">An AAttack with or without the moddata</param>
    /// <returns>Is splitshot's split shot?</returns>
    private static bool IsSplit(AAttack attack)
    {
        return ModEntry.Instance.Helper.ModData.TryGetModData<bool>(attack, "split", out var b) && b;
    }
}

/// <summary>
/// Splitshot class... why I didn't make this into a AAttack subclass? I forgot.
/// </summary>
public class ASplitshot : CardAction
{
    public int damage;
    public int? givesEnergy;  // Currently unused
    public Status? status;  // Currently unused
    public int statusAmount;  // Currently unused
    public Card? cardOnHit;  // Currently unused
    public CardDestination destination;  // Currently unused
    public bool stunEnemy;
    public int moveEnemy;  // Currently unused
    public bool weaken;  // Currently unused
    public bool brittle;  // Currently unused
    public bool armorise;  // Currently unused
    public bool piercing;
    public bool targetPlayer;
    public bool fast;
    public bool paybackAttack;  // Currently unused
    public bool multiCannonVolley;
    public int paybackCounter;  // Currently unused
    public bool storyFromStrafe;  // Currently unused
    public bool storyFromPayback;  // Currently unused
    public List<CardAction>? onKillActions;  // Currently unused
    public int? fromX;


    // TODO: Convert Splitshot to an AAttack subclass, calling the base Begin first then grabbing whatever relevant stuff is changed in AAttack like stun/pierce. May need to look into AAttack guards returning the stuff early for reasons


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
        bool librahit = fromShip.Get(Status.libra) > 0;

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
                if (toPierce == true)
                {
                    piercing = true;
                    artifact.Pulse();
                }
                bool? tostun = artifact.ModifyAttacksToStun(s, c);
                if (tostun == true)
                {
                    stunEnemy = true;
                    artifact.Pulse();
                }
            }
        }
        // Handle stunCharge if not a stun shot
        if (!stunEnemy && s.ship.Get(Status.stunCharge) > 0 && !targetPlayer)
        {
            s.ship.Set(Status.stunCharge, s.ship.Get(Status.stunCharge) - 1);
            stunEnemy = true;
        }

        // As of 0.3.43, Splitshot no longer handles the drone destruction. Instead it'll queue an AAttack to do it for it instead.
        bool hitADrone = false;
        if (raycastResult is not null && raycastResult.hitDrone)
        {
            hitADrone = true;
            // bool invincible = c.stuff[raycastResult.worldX].Invincible();
            // foreach (Artifact artifact in s.EnumerateAllArtifacts())
            // {
            //     bool? droneInvincibilityModified = artifact.ModifyDroneInvincibility(s, c, c.stuff[raycastResult.worldX]);
            //     if (droneInvincibilityModified == true)
            //     {
            //         invincible = true;
            //         artifact.Pulse();
            //     }
            // }
            // if (c.stuff[raycastResult.worldX].bubbleShield && !piercing)
            // {
            //     c.stuff[raycastResult.worldX].bubbleShield = false;
            // }
            // else if (invincible)
            // {
            //     c.QueueImmediate(c.stuff[raycastResult.worldX].GetActionsOnShotWhileInvincible(s, c, !targetPlayer, damage));
            // }
            // else
            // {
            //     c.DestroyDroneAt(s, raycastResult.worldX, !targetPlayer);
            // }
            //ModEntry.Instance.Logger.LogInformation("Split!");
            AAttack left = ConvertSplitToAttack(this);
            AAttack right = ConvertSplitToAttack(this);
            ModEntry.Instance.Helper.ModData.SetModData(left, "split", true);
            ModEntry.Instance.Helper.ModData.SetModData(right, "split", true);
            left.fast = right.fast = true;
            left.fromDroneX = raycastResult.worldX - 1;
            right.fromDroneX = raycastResult.worldX + 1;
            if (piercing)
            {
                AAttack center = ConvertSplitToAttack(this);
                center.fast = true;
                center.fromDroneX = raycastResult.worldX;
                ModEntry.Instance.Helper.ModData.SetModData(center, "split", true);
                if (librahit)
                {
                    c.QueueImmediate([left, GiveLibraEffect(fromShip), center, GiveLibraEffect(fromShip), right, GiveLibraEffect(fromShip)]);
                }
                else
                {
                    c.QueueImmediate([left, center, right]);
                }
            }
            else
            {
                if (librahit)
                {
                    c.QueueImmediate([left, GiveLibraEffect(fromShip), right, GiveLibraEffect(fromShip)]);
                }
                else
                {
                    c.QueueImmediate([left, right]);
                }
            }
        }
        AAttack origin = ConvertSplitToAttack(this);
        if (hitADrone)
        {
            ModEntry.Instance.Helper.ModData.SetModData(origin, "split", true);
            origin.fast = true;
        }
        c.QueueImmediate(origin);
        timer = 0.0;
    }



    public static AAttack ConvertSplitToAttack(ASplitshot splitshot, bool fake = false)
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
                xHint = splitshot.xHint,
                whoDidThis = splitshot.whoDidThis,
                artifactPulse = splitshot.artifactPulse,
                statusPulse = splitshot.statusPulse,
                omitFromTooltips = splitshot.omitFromTooltips,
                dialogueSelector = splitshot.dialogueSelector,
                disabled = splitshot.disabled,
                selectedCard = splitshot.selectedCard,
                canRunAfterKill = splitshot.canRunAfterKill,
                shardcost = splitshot.shardcost,
            };
        }
        AAttack aAttack = new()
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
            xHint = splitshot.xHint,
            whoDidThis = splitshot.whoDidThis,
            artifactPulse = splitshot.artifactPulse,
            statusPulse = splitshot.statusPulse,
            omitFromTooltips = splitshot.omitFromTooltips,
            dialogueSelector = splitshot.dialogueSelector,
            disabled = splitshot.disabled,
            selectedCard = splitshot.selectedCard,
            canRunAfterKill = splitshot.canRunAfterKill,
            shardcost = splitshot.shardcost,
        };
        ModEntry.Instance.Helper.ModData.CopyAllModData(splitshot, aAttack);
        return aAttack;
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
        if (status is Status stat)
        {
            tooltips.AddRange(StatusMeta.GetTooltips(stat, statusAmount));
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
            return new Icon(this.piercing ? ModEntry.Instance.SprSplitshotPiercing : ModEntry.Instance.SprSplitshot, damage, Colors.redd, false);
        }
        return new Icon(this.piercing ? ModEntry.Instance.SprSplitshotPiercingFail : ModEntry.Instance.SprSplitshotFail, damage, Colors.attackFail, false);
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
    private AStatus GiveLibraEffect(Ship source)
    {
        return new AStatus
        {
            targetPlayer = !this.targetPlayer,
            status = Status.tempShield,
            statusAmount = source.Get(Status.libra)
        };
    }
}