namespace FallingSandSimulator;

public abstract class Liquid : Cell
{
    public Liquid((float, float, float) colour, int x, int y) : base(colour, x, y)
    {
        displaceable = true;
    }

    public override void Update(World world)
    {
        int dir = RandDirection();
        (int, int)[] deltas = [(0, -1), (-1, -1), (1, -1), (-1, 0), (1, 0)];
        if(world.AttemptMoves(x, y, deltas, dir))
        {
            return;
        }
        if(world.AttemptDisplacements(x, y, deltas, dir))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}