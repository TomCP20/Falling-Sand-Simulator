using OpenTK.Mathematics;

namespace FallingSandSimulator;

public abstract class Cell
{
    public readonly Vector3 colour;

    private static Random rand = new();

    protected Cell(Vector3 colour)
    {
        this.colour = colour;
    }

    public static int RandDirection()
    {
        return new int[] { -1, 1 }[rand.Next(2)];
    }
    public abstract void Update(World world, int x, int y);
}