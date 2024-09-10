namespace FallingSandSimulator;

public abstract class Solid((float, float, float) colour) : Cell(colour)
{
    public override void Update(World world, int x, int y)
    {
        world.SetStepped(x, y);
    }
}