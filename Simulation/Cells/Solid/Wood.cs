namespace FallingSandSimulator;

public class Wood : Solid
{
    public Wood(int x, int y) : base(CellType.Wood, x, y)
    {
        burnChance = 0.05f;
    }
}