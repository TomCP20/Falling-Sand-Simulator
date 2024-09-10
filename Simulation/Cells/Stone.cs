namespace FallingSandSimulator;

public class Stone : Cell
{

    public Stone() : base(Colour.GreyNoise(0.5f, 0.1f)) { }

    public override void Update(World world, int x, int y)
    {
        world.SetStepped(x, y);
    }
}