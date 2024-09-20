namespace FallingSandSimulator;

public abstract class Cell(CellType type, int x, int y)
{
    public (float, float, float) colour = type.GetCol(x, y);

    public int x = x;

    public int y = y;

    public bool glow = false;

    protected static readonly Random rand = new();

    public bool displaceable = false;

    public float burnChance = 0;

    public float corrodeChance = 0.1f;

    public float infectChance = 0.1f;


    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }

    public static bool Random(float chance) // has a chance probability of returning true
    {
        return rand.NextSingle() <= chance;
    }

    public bool InLiquid(World world)
    {
        foreach (Cell? neighbor in GetNeighbors(world))
        {
            if (neighbor is Liquid)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerable<Cell?> GetNeighbors(World world)
    {
        (int, int)[] deltas = [(0, -1), (-1, 0), (1, 0), (0, 1)];
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx;
            int newy = y + dy;
            if (world.InBounds(newx, newy))
            {
                yield return world.GetCell(newx, newy);
            }
        }
    }

    public void Burn(World world)
    {
        world.SpawnCell(x, y, CellType.Fire);
    }

    public void Infect(World world)
    {
        world.SpawnCell(x, y, CellType.Virus);
    }
    public abstract void Update(World world);
}