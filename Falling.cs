using OpenTK.Mathematics;

namespace FallingSandSimulator;

public abstract class Falling : Cell
{
    protected Falling((float, float, float) colour) : base(colour) { }

    public override void Update(World world, int x, int y)
    {
        int dir = RandDirection();
        if (world.IsEmpty(x, y - 1))
        {
            world.MoveTo(x, y, x, y - 1);
        }
        else if (world.IsEmpty(x - dir, y - 1))
        {
            world.MoveTo(x, y, x - dir, y - 1);
        }
        else if (world.IsEmpty(x + dir, y - 1))
        {
            world.MoveTo(x, y, x + dir, y - 1);
        }
        else if (world.IsDisplaceable(x, y - 1))
        {
            world.Swap(x, y, x, y - 1);
        }
        else if (world.IsDisplaceable(x - dir, y - 1))
        {
            world.Swap(x, y, x - dir, y - 1);
        }
        else if (world.IsDisplaceable(x + dir, y - 1))
        {
            world.Swap(x, y, x + dir, y - 1);
        }
        else
        {
            world.SetStepped(x, y);
        }
    }
}