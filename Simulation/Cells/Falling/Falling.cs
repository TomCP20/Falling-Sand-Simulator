namespace FallingSandSimulator;

public abstract class Falling((float, float, float) colour) : Cell(colour)
{
    public override void Update(World world, int x, int y)
    {
        int dir = RandDirection();
        (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
        (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
        if(world.AttemptMoves(x, y, deltas1, dir))
        {
            return;
        }
        if(world.AttemptDisplacements(x, y, deltas2, dir))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}