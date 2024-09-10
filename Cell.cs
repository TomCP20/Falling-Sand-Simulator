namespace FallingSandSimulator;

public abstract class Cell((float, float, float) colour)
{
    public readonly (float, float, float) colour = colour;

    protected static readonly Random rand = new();

    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }
    public abstract void Update(World world, int x, int y);

    public bool AttemptMoves(World world, int x, int y, (int, int)[] deltas)
    {
        int dir = RandDirection();
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (world.IsEmpty(newx, newy))
            {
                world.MoveTo(x, y, newx, newy);
                return true;
            }
        }
        return false;
    }

    public bool AttemptDisplacements(World world, int x, int y, (int, int)[] deltas)
    {
        int dir = RandDirection();
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (world.IsDisplaceable(newx, newy))
            {
                world.Swap(x, y, newx, newy);
                return true;
            }
        }
        return false;
    }
}