namespace FallingSandSimulator;

public class Fire : Solid
{
    public Fire(int x, int y) : base(Colour.RandomMix(Colour.Red, Colour.Orange), x, y) { }

    private static readonly float decayChance = 0.01f;

    private static readonly float smokeChance = 0.1f;

    public override void Update(World world)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(x, y);
            return;
        }
        if (world.InBounds(x, y + 1) && world.GetCell(x, y + 1) == null)
        {
            if (Random(smokeChance))
            {
                world.SpawnCell(x, y + 1, CellType.Smoke);
            }
        }
        foreach ((int nx, int ny) in world.GetNeighbors(x, y))
        {
            Cell? neighbor = world.GetCell(nx, ny);
            if (neighbor != null)
            {
                if (Random(neighbor.burnChance))
                {
                    world.SpawnCell(nx, ny, CellType.Fire);
                }
            }
        }
        base.Update(world);
    }
}