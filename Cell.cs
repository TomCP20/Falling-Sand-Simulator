namespace FallingSandSimulator;

public abstract class Cell((float, float, float) colour)
{
    public readonly (float, float, float) colour = colour;

    private static readonly Random rand = new();

    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }
    public abstract void Update(World world, int x, int y);
}