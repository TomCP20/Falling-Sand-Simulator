namespace FallingSandSimulator;

public class Brick : Solid
{
    public Brick() : base(Colour.Red) { }

    public override void Update(World world, int x, int y)
    {
        colour = Colour.BrickPattern(x, y);

        base.Update(world, x, y);
    }
}