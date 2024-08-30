using OpenTK.Mathematics;

namespace FallingSandSimulator;

public abstract class Cell
{
    public readonly Vector3 colour;

    protected Cell(Vector3 colour)
    {
        this.colour = colour;
    }

    public abstract void Update(World world, int x, int y);
}