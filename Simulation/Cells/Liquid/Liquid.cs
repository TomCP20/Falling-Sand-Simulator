namespace FallingSandSimulator;

public abstract class Liquid((float, float, float) colour) : Cell(colour)
{
    public override void Update(World world, int x, int y)
    {
        int dir = RandDirection();
        (int, int)[] deltas = [(0, -1), (-1, -1), (1, -1), (-1, 0), (1, 0)];
        if(world.AttemptMoves(x, y, deltas, dir))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}