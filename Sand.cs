using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Sand : Cell
{

    public Sand() : base((1, 1, 0)) { }

    protected Sand((float, float, float) colour) : base(colour) { }

    public override void Update(World world, int x, int y)
    {
        int dir = RandDirection();
        if (world.IsDisplaceable(x, y - 1))
        {
            world.Swap(x, y, x, y - 1);
            world.SetStepped(x, y - 1);
        }
        else if (world.IsDisplaceable(x - dir, y - 1))
        {
            world.Swap(x, y, x - dir, y - 1);
            world.SetStepped(x - dir, y - 1);
        }
        else if (world.IsDisplaceable(x + dir, y - 1))
        {
            world.Swap(x, y, x + dir, y - 1);
            world.SetStepped(x + dir, y - 1);
        }
        else
        {
            world.SetStepped(x, y);
        }
    }
}