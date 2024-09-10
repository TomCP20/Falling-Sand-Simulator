namespace FallingSandSimulator;

public class Fire : Solid
{
    public Fire() : base((1, 0.2f, 0)) { }

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
        
        base.Update(world, x, y);
    }
}