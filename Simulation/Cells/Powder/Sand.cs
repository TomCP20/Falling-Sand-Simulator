namespace FallingSandSimulator;

public class Sand : Powder
{
    public Sand(int x, int y) : base(Colour.Noise(Colour.Yellow, 0.1f), x, y) { }
}