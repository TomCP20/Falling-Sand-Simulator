namespace FallingSandSimulator;

public abstract class Cell((float, float, float) colour)
{
    public readonly (float, float, float) colour = colour;

    protected static readonly Random rand = new();

    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }

    public static bool Random(float chance) // has a chance probability of returning true
    {
        return rand.NextSingle() <= chance;
    }
    public abstract void Update(World world, int x, int y);
}