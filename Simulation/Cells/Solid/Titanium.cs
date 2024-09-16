namespace FallingSandSimulator;

public class Titanium : Solid
{
    public Titanium(int x, int y) : base(Colour.White, x, y)
    {
        corrodeChance = 0;
    }
}