using OpenTK.Audio.OpenAL;

namespace FallingSandSimulator;

public abstract class Liquid : Moving
{
    private readonly float dispersion = 15;
    protected Liquid(CellType type, int x, int y) : base(type, x, y)
    {
        deltas = [(0, -1), (-1, -1), (1, -1)];
    }

    public override bool Update(World world)
    {
        if(base.Update(world))
        {
            return true;
        }

        int dir = RandDirection();
        if (Disperse(world, dir))
        {
            return true;
        }
        if (Disperse(world, -dir))
        {
            return true;
        }

        return false;
    }

    private bool Disperse(World world, int dir)
    {
        int maxMove = 0;
        for (int i = 1; i <= dispersion; i++)
        {
            int nx = x + i * dir;
            if (world.IsEmpty(nx, y) && ! world.IsEmpty(nx, y-1))
            {
                maxMove = i;
            }
            else 
            {
                break;
            }
        }
        if (maxMove != 0)
        {
            world.MoveTo(this, x + maxMove * dir, y);
            return true;
        }
        return false;
    }
}