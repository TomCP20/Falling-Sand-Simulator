using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Cell
{
    public readonly Vector3 col;

    public Cell(Vector3 col)
    {
        this.col = col;
    }
}