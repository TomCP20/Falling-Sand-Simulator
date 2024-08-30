using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Water : Cell
{

    public Water() : base(new Vector3(0, 0, 1)) { }


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
        else if (world.IsEmpty(x-dir, y))
        {
            world.Swap(x, y, x-dir, y);
            world.SetStepped(x-dir, y);
        }
        else if (world.IsEmpty(x+dir, y))
        {
            world.Swap(x, y, x+dir, y);
            world.SetStepped(x+dir, y);
        }
        else
        {
            world.SetStepped(x, y);
        }
    }
}