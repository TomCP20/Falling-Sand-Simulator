namespace FallingSandSimulator;

public class Gravel : Powder
{
    public Gravel(int x, int y) : base(CellType.Gravel, x, y)
    {
        deltas1 = [(0, -1)];
        deltas2 = [(0, -1)];
    }
}