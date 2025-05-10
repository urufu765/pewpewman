
using Nickel;

namespace Weth.Actions;

public class ASodaOpen : CardAction
{
    public ISoundEntry? sound;
    public float? vol;
    public float? pitch;
    public override void Begin(G g, State s, Combat c)
    {
        if (sound is ISoundEntry sfx)
        {
            ISoundInstance isi = sfx.CreateInstance();
            isi.Volume = vol??1;
            isi.Pitch = pitch??1;
        }
    }
}