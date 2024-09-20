using System.Diagnostics;

namespace FallingSandSimulator;

public abstract class Moving(CellType type, int x, int y) : Cell(type, x, y)
{

    protected (int, int)[] deltas = [];

    public override bool Update(World world)
    {
        int dir = RandDirection();
        
        if(AttemptMoves(world, deltas, dir))
        {
            return true;
        }
        if(AttemptDisplacements(world, deltas, dir))
        {
            return true;
        }
        return base.Update(world);
    }

    public bool AttemptMoves(World world, (int, int)[] deltas, int dir)
    {
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (world.IsEmpty(newx, newy))
            {
                world.MoveTo(this, newx, newy);
                return true;
            }
        }
        return false;
    }

    public bool AttemptDisplacements(World world, (int, int)[] deltas, int dir)
    {
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (world.InBounds(newx, newy))
            {
                Cell? cell = world.GetCell(newx, newy);
                Debug.Assert(cell != null);
                if (cell.displaceable)
                {
                    world.Swap(this, cell);
                    return true;
                }
            }
        }
        return false;
    }
}