namespace FallingSandSimulator;

public class Titanium : Solid
{
    public Titanium(int x, int y) : base(CellType.Titanium, x, y)
    {
        corrodeChance = 0;
    }
}