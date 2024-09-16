namespace FallingSandSimulator;

public class Brick : Solid
{
    public Brick(int x, int y) : base(Colour.Red, x, y) { }

    public override void Update(World world)
    {
        colour = Colour.BrickPattern(x, y);

        base.Update(world);
    }
}