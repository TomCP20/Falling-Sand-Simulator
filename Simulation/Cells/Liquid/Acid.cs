namespace FallingSandSimulator;

public class Acid : Liquid
{

    public float expireChance = 0.5f;

    public Acid(int x, int y) : base(Colour.Green, x, y)
    {
        corrodeChance = 0;
        infectChance = 0;
        glow = true;
    }

    public override void Update(World world)
    {
        foreach (Cell? neighbor in GetNeighbors(world))
        {
            if (neighbor != null)
            {
                if (Random(neighbor.corrodeChance))
                {
                    world.DeleteCell(neighbor);
                    if (Random(expireChance))
                    {
                        world.DeleteCell(this);
                        return;
                    }
                }
            }
        }
        base.Update(world);
    }
}