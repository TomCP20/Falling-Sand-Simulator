namespace FallingSandSimulator;

public abstract class Powder((float, float, float) colour, int x, int y) : Cell(colour, x, y)
{
    protected (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
    protected (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
    public override void Update(World world)
    {
        int dir = RandDirection();
        if(AttemptMoves(world, deltas1, dir))
        {
            return;
        }
        if(AttemptDisplacements(world, deltas2, dir))
        {
            return;
        }
        world.SetStepped(this);
    }
}