namespace FallingSandSimulator;

public abstract class Powder((float, float, float) colour, int x, int y) : Cell(colour, x, y)
{
    protected (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
    protected (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
    public override void Update(World world)
    {
        int dir = RandDirection();
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