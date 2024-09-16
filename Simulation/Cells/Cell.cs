namespace FallingSandSimulator;

public abstract class Cell((float, float, float) colour, int x, int y)
{
    public (float, float, float) colour = colour;

    public int x = x;

    public int y = y;

    protected static readonly Random rand = new();

    public bool displaceable = false;

    public float burnChance = 0;

    public float corrodeChance = 0.1f;



    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }

    public static bool Random(float chance) // has a chance probability of returning true
    {
        return rand.NextSingle() <= chance;
    }
    public abstract void Update(World world);
}