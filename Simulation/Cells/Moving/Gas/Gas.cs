namespace FallingSandSimulator;

public abstract class Gas : Moving
{
    public Gas(CellType type, int x, int y) : base(type, x, y)
    {
        displaceable = true;
        deltas = [(-1, 1), (1, 1), (0, 1), (-1, 0), (1, 0)];
    }
}