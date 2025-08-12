using System;
using FMOD;
using FSPRO;

namespace Weth.Conversation;

public class BGWethVault : BG
{
    private bool peekTransition;
    private double peekTransitionTimer;
    private const double peekTransitionDuration = 30.0;
    private bool transition;
    private double transitionTimer;
    private const double transitionDuration = 15.0;
    private bool rumble;
    private bool rumbleIntensify;
    private double rumbleTimer;
    private double shardTimer;
    private bool shardHit;
    private double bangTimer;
    private double age;
    private bool autoAdvance;
    public bool ForceAdvanceDialogue { get { return (age > 4) || autoAdvance; } }
    private ParticleSystem particles = new ParticleSystem{blend = BlendMode.Add};
    public override void Render(G g, double t, Vec offset)
    {
        if (peekTransition)
        {
            peekTransitionTimer += g.dt;
        }
        if (transition)
        {
            transitionTimer += g.dt;
        }
        Vec lookaway = new Vec(
            Mutil.Lerp(
                Mutil.Lerp(
                    G.screenSize.x,
                    400,
                    Helpers.InverseLerp(0, peekTransitionDuration, peekTransitionTimer)
                ),
                150,
                ILerpEaseOut(0, transitionDuration, transitionTimer)
            ), 0
        );
        BGVault.DrawVaultBg(g, lookaway, letterbox: true, voidFragments: rumbleIntensify);

        if (rumble)
        {
            if (rumbleIntensify)  // OH NO IT BLEW UP
            {
                rumbleTimer += g.dt;
                age += g.dt;
                Draw.Fill(new Color(0.25, 0.5, 1).gain(rumbleTimer / 3.5), BlendMode.Screen);
                Draw.Fill(Colors.white.fadeAlpha(rumbleTimer / 5));
                Audio.Auto(Event.Scenes_CobaltCritical);
            }
            else  // Casual shaking
            {
                rumbleTimer = 0.2;
            }
            Audio.Auto(Event.Scenes_CoreAlarmFromOutside);
            g.state.shake = rumbleTimer;
        }

        // Explosion!
        if (bangTimer > 0)
        {
            g.state.shake += bangTimer;
            Draw.Fill(new Color(1, 0.92, 0.85).fadeAlpha(bangTimer / 6));
            bangTimer = Math.Max(0, bangTimer - g.dt);
        }


        // Shard in face!
        if (shardTimer > 0)
        {
            double circal = Math.Sin(Mutil.Rand(shardTimer * 8 * 0.123 * Math.PI));
            Draw.Line(lookaway.x + 233, 130, 0, 100, shardTimer * 4, new Color("12b7ff"));
            particles.Add(
                new Particle
                {
                    pos = Vec.Lerp(new Vec(lookaway.x + 233, 130), new Vec(0, 100 + (circal * 32)), Math.Pow(Mutil.NextRand(), 3)),
                    vel = Mutil.RandVel() * 3,
                    lifetime = 1.0 * Mutil.NextRand(),
                    size = 1,
                    color = new Color("12b7ff")
                }
            );
            if (shardHit)
            {
                PlayerScreenDamage.OneShot();
                Audio.Play(new GUID?(Event.Hits_HitHurt), true);
                shardHit = false;
            }
            particles.Render(g.dt);
            shardTimer = Math.Max(0, shardTimer - g.dt);
            //BGComponents.Letterbox();
        }
    }

    private static double ILerpEaseOut(double a, double b, double n)
    {
        if (a == b) return 0;
        return Math.Sin(Mutil.Lerp(0, Math.PI/2, Helpers.InverseLerp(a, b, n)));
    }

    public override void OnAction(State s, string action)
    {
        switch (action)
        {
            case "rumble_on":
                rumble = true;
                break;
            case "rumble_intensify":
                rumbleIntensify = true;
                break;
            case "bang":
                bangTimer = 2.5;
                break;
            case "shard":
                shardTimer = 1;
                bangTimer = 1.5;
                shardHit = true;
                WethArtAndFrameSwitcher.SetWethCharFrame(1);
                break;
            case "peek":
                peekTransition = true;
                break;
            case "transition":
                peekTransition = false;
                transition = true;
                break;
            case "stop":
                rumble = rumbleIntensify = transition = false;
                s.shake = age = 0;
                break;
            case "auto_advance_on":
                autoAdvance = true;
                break;
            case "auto_advance_off":
                autoAdvance = false;
                break;
        }
    }
}