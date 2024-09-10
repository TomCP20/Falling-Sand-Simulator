namespace FallingSandSimulator;

public class Water : Cell
{

    public Water() : base((0, 0, 1)) { }


    public override void Update(World world, int x, int y)
    {
        (int, int)[] deltas = [(0, -1), (-1, -1), (1, -1), (-1, 0), (1, 0)];
        if(AttemptMoves(world, x, y, deltas))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}