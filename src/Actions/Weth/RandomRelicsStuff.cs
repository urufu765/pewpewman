using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Weth.Artifacts;

namespace Weth.Actions;

public class AGainRelicsRandom : CardAction
{
    public required int count;
    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        List<Artifact> workingList = s.EnumerateAllArtifacts().FindAll(a => a is WethRelicFour);

        try
        {
            for (int i = 0; i < count; i++)
            {
                if (
                    Activator.CreateInstance(
                        ModEntry.NewRegularRelicCounterparts[
                            workingList.Shuffle(s.rngActions).First().GetType()
                        ]
                    ) is Artifact a
                )
                {
                    s.GetCurrentQueue().QueueImmediate(
                        new AWethSingleArtifactOffering
                        {
                            canSkip = false,
                            artifact = a
                        }
                    );
                }
            }
        }
        catch (Exception err)
        {
            ModEntry.Instance.Logger.LogError(err, "OH NO RANDOM RELIC BADDDD");
        }
    }
}