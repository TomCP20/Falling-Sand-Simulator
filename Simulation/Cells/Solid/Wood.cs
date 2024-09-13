namespace FallingSandSimulator;

public class Wood : Solid
{
    public Wood() : base(Colour.Noise(Colour.Brown, 0.02f))
    {
        burnChance = 0.02f;
    }
}