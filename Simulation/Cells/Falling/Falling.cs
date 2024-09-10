namespace FallingSandSimulator;

public abstract class Falling((float, float, float) colour) : Cell(colour)
{
    public override void Update(World world, int x, int y)
    {
        (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
        (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
        if(AttemptMoves(world, x, y, deltas1))
        {
            return;
        }
        if(AttemptDisplacements(world, x, y, deltas2))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}