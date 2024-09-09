namespace FallingSandSimulator;

public class Water : Cell
{

    public Water() : base((0, 0, 1)) { }


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
        else if (world.IsEmpty(x - dir, y))
        {
            world.MoveTo(x, y, x - dir, y);
        }
        else if (world.IsEmpty(x + dir, y))
        {
            world.MoveTo(x, y, x + dir, y);
        }
        else
        {
            world.SetStepped(x, y);
        }
    }
}