namespace FallingSandSimulator;

public class Smoke(int x, int y) : Gas(CellType.Smoke, x, y)
{
    private static readonly float decayChance = 0.01f;

    public override bool Update(World world)
    {
        if (Random(decayChance))
        {
            world.DeleteCell(this);
            return true;
        }
        return base.Update(world);
    }
    
}