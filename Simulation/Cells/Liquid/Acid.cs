namespace FallingSandSimulator;

public class Acid : Liquid
{

    public float expireChance = 0.5f;

    public Acid() : base(Colour.Green)
    {
        corrodeChance = 0;
    }

    public override void Update(World world, int x, int y)
    {
        foreach ((int nx, int ny) in world.GetNeighbors(x, y))
        {
            Cell? neighbor = world.GetCell(nx, ny);
            if (neighbor != null)
            {
                if (Random(neighbor.corrodeChance))
                {
                    world.DeleteCell(nx, ny);
                    if (Random(expireChance))
                    {
                        world.DeleteCell(x, y);
                        return;
                    }
                }
            }
        }
        base.Update(world, x, y);
    }
}