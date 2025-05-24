using System;
using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Weth;
using Weth.Conversation;
public static class WethForceAdvanceDialogue
{
    private static double breatheTime;

    /// <summary>
    /// Force-advances the dialogue and doesn't wait for player input
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="g"></param>
    public static void ForceDialogueOnScream(Dialogue __instance, G g)
    {
        if (!__instance.alreadyAdvancedThisFrame && __instance.bg is BGWethVault bwv && bwv.ForceAdvanceDialogue)
        {
            __instance.alreadyAdvancedThisFrame = true;
            __instance.OnPlayerAdvanceInput(g);
        }
    }

    /// <summary>
    /// Character.DrawFace
    /// </summary>
    public static void DrawWethCharOverlay(Character __instance, G g, double x, double y, bool flipX, string animTag, bool mini, bool renderLocked, bool hideFace)
    {
        if (__instance.type != ModEntry.WethTheSnep.CharacterType) return;
        Color glowColor = new("7fffff");
        breatheTime += g.dt;

        // Character sprite's crystal glow
        if (!renderLocked && !hideFace)
        {
            if (mini)
            {
                UhDuhHundo.ApplySubtleCrystalOverlayGlow(new Vec(x, y), GetSpriteOverlayGlowSpots(animTag), glowColor, breatheTime, cycleTime: 5, maxGlow: 0.35);
                return;
            }
            else if (RenderExtraGlow(animTag) is bool renderExtra)
            {
                UhDuhHundo.ApplySubtleCrystalOverlayGlow(new Vec(x, y), GetSpriteOverlayGlowSpots(animTag), glowColor, breatheTime, cascade: true, cycleTime: 4.1, maxGlow: 0.4, extraSize: new(4, 2));
                if (renderExtra)
                {
                    if (animTag.StartsWith("crystal"))
                    {
                        UhDuhHundo.ApplySubtleCrystalOverlayGlow(new Vec(x, y), GetExtraSpriteOverlayGlowSpots(animTag), glowColor, breatheTime, cascade: true, cycleTime: 3, maxGlow: 0.5, extraSize: new(5, 7));
                    }
                }
            }
        }

        // Full border overlay & glow
        if (
            !mini &&
            DB.charPanels.TryGetValue(__instance.type, out Spr frame) &&
            WethArtAndFrameSwitcher.GetWethCharIndexFromFrame(frame) is int i &&
            WethArtAndFrameSwitcher.GetWethCharOverlay(i) is Spr overlay
        )
        {
            Draw.Sprite(overlay, x, y, flipX);
            UhDuhHundo.ApplySubtleCrystalOverlayGlow(new Vec(x, y), GetFrameOverlayGlowSpots(i), glowColor, breatheTime, maxGlow: 0.4);
        }
    }

    private static (Vec, Vec)[] GetSpriteOverlayGlowSpots(string animTag)
    {
        return animTag switch
        {
            "mini" => [(new(26, 14), new(6, 7))],
            "cry" => [
                (new(49, 15), new(7, 9)),
                (new(57, 17), new(7, 5)),
                (new(49, 23), new(7, 5)),
                (new(54, 27), new(11, 7)),
                ],
            "facepalm" => [(new(49, 44), new(9, 13))],
            "up" => [
                (new(26, 24), new(7, 5)),
                (new(32, 20), new(9, 3)),
                (new(38, 18), new(7, 5)),
                (new(37, 25), new(17, 9)),
                (new(46, 22), new(6, 5)),
                (new(45, 29), new(7, 5)),
                (new(51, 25), new(5, 5)),
                (new(43, 36), new(13, 7)),
                (new(52, 31), new(7, 5)),
                (new(51, 35), new(7, 5)),
                ],
            "down" => [
                (new(37, 23), new(3, 3)),
                (new(42, 25), new(10, 4)),
                (new(49, 28), new(5, 5)),
                (new(48, 32), new(11, 4)),
                (new(44, 36), new(11, 7)),
                ],
            _ => [(new(47, 28), new(10, 10))],
        };
    }

    private static (Vec, Vec)[] GetExtraSpriteOverlayGlowSpots(string animTag)
    {
        return animTag switch
        {
            string tag when tag.StartsWith("crystal") => [
                (new(55, 37), new(9, 12)),
                (new(59, 49), new(4, 7)),
                (new(57, 42), new(5, 8))
                ],
            _ => []
        };
    }

    private static bool? RenderExtraGlow(string animTag)
    {
        switch (animTag)
        {
            // Just don't render glow plz
            case string tag when tag.StartsWith("past"):
            case "crystallized":
            case "gameover":
            case "outsidetest":
            case "placeholder":
            case "squintoffscreen":
                return null;
            // Render the eye crystal AND the crystal Weth is holding glow
            case string tag when tag.StartsWith("crystal"):
                return true;
            // Render the eye crystal glow
            default:
                return false;
        }
    }

    private static (Vec, Vec)[] GetFrameOverlayGlowSpots(int n = 0)
    {
        return n switch
        {
            3 => [
                (new(5, 32), new(10, 40)),
                (new(17, 3), new(10, 4)),
                (new(26, 58), new(11, 4)),
                (new(57, 58), new(9, 4)),
                (new(61, 5), new(5, 10)),
                (new(61, 40), new(4, 9)),
            ],
            2 => [
                (new(5, 32), new(15, 30)),
                (new(18, 2), new(3, 2)),
                (new(24, 59), new(3, 2)),
                (new(57, 58), new(6, 4)),
                (new(61, 8), new(4, 4)),
                (new(61, 40), new(3, 3)),
            ],
            1 => [
                (new(4, 30), new(10, 20))
            ],
            _ => []
        };
    }
}