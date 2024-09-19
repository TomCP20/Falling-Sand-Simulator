namespace FallingSandSimulator;

public class Fire : Solid
{
    public Fire(int x, int y) : base(Colour.RandomMix(Colour.Red, Colour.Yellow), x, y)
    {
        glow = true;
        infectChance = 0;
    }

    private static readonly float decayChance = 0.01f;

    private static readonly float smokeChance = 0.1f;

    public override void Update(World world)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(this);
            return;
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
        base.Update(world);
    }
}