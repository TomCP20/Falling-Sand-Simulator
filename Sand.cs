using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Sand : Cell
{

    public Sand() : base(new Vector3(1, 1, 0)) { }

    protected Sand(Vector3 colour) : base(colour) { }

    public override void Update(World world, int x, int y)
    {
        int dir = RandDirection();
        if (world.IsEmpty(x, y-1))
        {
            world.Swap(x, y, x, y-1);
            world.SetStepped(x, y-1);
        }
        else if (world.IsEmpty(x-dir, y-1))
        {
            world.Swap(x, y, x-dir, y-1);
            world.SetStepped(x-dir, y-1);
        }
        else if (world.IsEmpty(x+dir, y-1))
        {
            world.Swap(x, y, x+dir, y-1);
            world.SetStepped(x+dir, y-1);
        }
        else
        {
            world.SetStepped(x, y);
        }
    }
}