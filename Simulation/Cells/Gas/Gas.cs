using System.Security.Cryptography.X509Certificates;

namespace FallingSandSimulator;

public abstract class Gas : Cell
{
    public Gas((float, float, float) colour, int x, int y) : base(colour, x, y)
    {
        displaceable = true;
    }
    public override void Update(World world)
    {
        int dir = RandDirection();
        (int, int)[] deltas = [(-1, 1), (1, 1), (0, 1), (-1, 0), (1, 0)];
        if(world.AttemptMoves(x, y, deltas, dir))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}