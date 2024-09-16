namespace FallingSandSimulator;

public class Sand : Powder
{
    public Sand() : base(Colour.Noise(Colour.Yellow, 0.1f)) { }
}