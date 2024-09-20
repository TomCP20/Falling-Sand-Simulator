namespace FallingSandSimulator;

public abstract class Solid(CellType type, int x, int y) : Cell(type, x, y)
{
    public override void Update(World world)
    {
        world.SetStepped(this);
    }
}