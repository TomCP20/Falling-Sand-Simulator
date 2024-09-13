namespace FallingSandSimulator;

public class Sand : Falling
{
    public Sand() : base(Colour.Noise(Colour.Yellow, 0.1f)) { }
}