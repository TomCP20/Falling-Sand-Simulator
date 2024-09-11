namespace FallingSandSimulator;

public class Fire : Solid
{
    public Fire() : base(Colour.RandomMix((1, 0, 0), (1, 0.64f, 0))) { }

    private static readonly float decayChance = 0.01f;

    private static readonly float smokeChance = 0.1f;

    public override void Update(World world, int x, int y)
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
                world.SpawnCell<Smoke>(x, y + 1);
            }
        }
        foreach ((int nx, int ny) in world.GetNeighbors(x, y))
        {
            Cell? neighbor = world.GetCell(nx, ny);
            if (neighbor != null)
            {
                if (Random(neighbor.burnChance))
                {
                    world.SpawnCell<Fire>(nx, ny);
                }
            }
        }
        base.Update(world, x, y);
    }
}