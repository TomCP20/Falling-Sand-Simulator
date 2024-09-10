namespace FallingSandSimulator;

public abstract class Falling((float, float, float) colour) : Cell(colour)
{
    public override void Update(World world, int x, int y)
    {
        (int, int)[] deltas = [(0, -1), (-1, -1), (1, -1)];
        if(AttemptMoves(world, x, y, deltas))
        {
            return;
        }
        if(AttemptDisplacements(world, x, y, deltas))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}