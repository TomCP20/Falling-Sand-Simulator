namespace FallingSandSimulator;

public abstract class Gas : Cell
{
    public Gas(CellType type, int x, int y) : base(type, x, y)
    {
        displaceable = true;
    }
    public override void Update(World world)
    {
        int dir = RandDirection();
        (int, int)[] deltas = [(-1, 1), (1, 1), (0, 1), (-1, 0), (1, 0)];
        if(AttemptMoves(world, deltas, dir))
        {
            return;
        }
        world.SetStepped(this);
    }
}