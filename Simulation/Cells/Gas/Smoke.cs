namespace FallingSandSimulator;

public class Smoke : Gas
{
    public Smoke() : base((0.51f, 0.53f, 0.51f)) { }

    private static readonly float decayChance = 0.01f;

    public override void Update(World world, int x, int y)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(x, y);
            return;
        }
        base.Update(world, x, y);
    }
    
}