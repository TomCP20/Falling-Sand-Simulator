namespace FallingSandSimulator;

public abstract class Liquid : Moving
{
    public Liquid(CellType type, int x, int y) : base(type, x, y)
    {
        displaceable = true;
        deltas = [(0, -1), (-1, -1), (1, -1), (-1, 0), (1, 0)];
    }
}