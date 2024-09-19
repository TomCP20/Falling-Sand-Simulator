namespace FallingSandSimulator;

public class Wood : Solid
{
    public Wood(int x, int y) : base(Colour.Noise(Colour.Brown, 0.02f), x, y)
    {
        burnChance = 0.05f;
    }
}