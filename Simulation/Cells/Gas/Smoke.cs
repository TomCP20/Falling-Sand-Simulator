namespace FallingSandSimulator;

public class Smoke(int x, int y) : Gas(CellType.Smoke, x, y)
{
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