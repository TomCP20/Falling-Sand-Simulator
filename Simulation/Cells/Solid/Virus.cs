namespace FallingSandSimulator;

public class Virus : Solid
{
    public Virus(int x, int y) : base(Colour.Noise(Colour.Purple, 0.4f), x, y)
    {
        infectChance = 0;
        burnChance = 0.5f;
        glow = true;
    }

    public override void Update(World world)
    {
        foreach (Cell? neighbor in GetNeighbors(world))
        {
            if (neighbor != null)
            {
                if (Random(neighbor.infectChance))
                {
                    neighbor.Infect(world);
                }
            }
        }
        base.Update(world);
    }
}