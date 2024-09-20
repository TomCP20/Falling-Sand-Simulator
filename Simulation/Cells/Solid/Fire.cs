namespace FallingSandSimulator;

public class Fire : Solid
{
    public Fire(int x, int y) : base(CellType.Fire, x, y)
    {
        glow = true;
        infectChance = 0;
    }

    private static readonly float baseDecayChance = 0.01f;

    private static readonly float smokeChance = 0.1f;

    public override bool Update(World world)
    {
        float decayChance = baseDecayChance *  (InWater(world) ? 10 : 1);
        if (Random(decayChance))
        {
            world.DeleteCell(this);
            return true;
        }
        if (world.IsEmpty(x, y + 1))
        {
            if (Random(smokeChance))
            {
                world.SpawnCell(x, y + 1, CellType.Smoke);
            }
        }
        foreach (Cell? neighbor in GetNeighbors(world))
        {
            if (neighbor != null)
            {
                if (Random(neighbor.burnChance))
                {
                    neighbor.Burn(world);
                }
            }
        }
        return base.Update(world);
    }
}