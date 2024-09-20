namespace FallingSandSimulator;

public abstract class Powder(CellType type, int x, int y) : Cell(type, x, y)
{
    protected (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
    protected (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
    public override void Update(World world)
    {
        int dir = RandDirection();
        (int, int)[] deltas;
        if (InLiquid(world))
        {
            deltas = deltas2;
        }
        else
        {
            deltas = deltas1;
        }
        if(AttemptMoves(world, deltas, dir))
        {
            return;
        }
        if(AttemptDisplacements(world, deltas, dir))
        {
            return;
        }
        world.SetStepped(this);
    }
}