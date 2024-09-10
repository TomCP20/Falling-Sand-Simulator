namespace FallingSandSimulator;

public class Smoke : Cell
{
    public Smoke() : base((0.51f, 0.53f, 0.51f)) { }

    private static readonly float decayChance = 0.01f;


    public override void Update(World world, int x, int y)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(x, y);
            return;
        }
        int dir = RandDirection();
        (int, int)[] deltas = [(-1, 1), (1, 1), (0, 1), (-1, 0), (1, 0)];
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