namespace Weth.Dialogue;

public class BGWethShop : BGShop
{
    public override void Render(G g, double t, Vec offset)
    {
        base.Render(g, t, offset);
        BGComponents.Letterbox();
    }
}