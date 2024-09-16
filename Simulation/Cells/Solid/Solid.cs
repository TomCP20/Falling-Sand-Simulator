namespace FallingSandSimulator;

public abstract class Solid((float, float, float) colour, int x, int y) : Cell(colour, x, y)
{
    public override void Update(World world)
    {
        world.SetStepped(x, y);
    }
}