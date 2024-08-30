using OpenTK.Mathematics;

namespace FallingSandSimulator;

public abstract class Cell
{
    public readonly Vector3 col;

    protected Cell(Vector3 col)
    {
        this.col = col;
    }

    public abstract void Update(World world, int x, int y);
}