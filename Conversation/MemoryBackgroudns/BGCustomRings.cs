using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static Weth.Dialogue.CommonDefinitions;

namespace Weth.Dialogue;

public class BGWethRings : BGRings
{
    private bool rumble;
    private double rumbleTimer;
    private double halfPoint;
    private double flashTimer;
    // public static void Apply(Harmony harmony)
    // {
    //     harmony.Patch(
    //         original: typeof(Events).GetMethod(nameof(Events.RunWinWho), AccessTools.all),
    //         postfix: new HarmonyMethod(typeof(WethEndingArtSwitcher), nameof(SwitchTheArt))
    //     );
    // }
    public override void Render(G g, double t, Vec offset)
    {
        base.Render(g, t, offset);
        if (rumble)
        {
            rumbleTimer -= g.dt;
            if (rumbleTimer <= 0)
            {
                rumbleTimer = 1.0 + Mutil.NextRand() * 3.0;
                halfPoint = rumbleTimer / 2;
                PlayerScreenDamage.OneShot();
                g.state.shake += 0.5;
            }
            Draw.Fill(new Color(1, 0.6, 0.4, 1.0).gain(Math.Max(0, rumbleTimer - halfPoint) / 8.0), BlendMode.Screen);
        }
        if (flashTimer > 0.0)
        {
            //g.state.shake += flashTimer;
            Draw.Fill(new Color(1, 0.6, 0.4, 1.0).gain(flashTimer / 3.0), BlendMode.Screen);
            Draw.Fill(new Color(1, 0.6, 0.4, 1.0).fadeAlpha(flashTimer / 4.0));
            flashTimer -= g.dt;
        }
    }

    public override void OnAction(State s, string action)
    {
        switch (action)
        {
            case "rumble_on":
                rumble = true;
                break;
            case "flash_weak":
                flashTimer = 1;
                PlayerScreenDamage.OneShot();
                s.shake = 2;
                break;
            case "flash":
                flashTimer = 2;
                s.shake = 3;
                break;
            case "bonk":
                s.shake += 0.4;
                break;
        }
    }
}