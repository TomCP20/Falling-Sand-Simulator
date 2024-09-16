namespace FallingSandSimulator;

public class Smoke : Gas
{
    public Smoke(int x, int y) : base(Colour.SmokeGrey, x, y) { }

    private static readonly float decayChance = 0.01f;

    public override void Update(World world)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(this);
            return;
        }
        base.Update(world);
    }
    
}