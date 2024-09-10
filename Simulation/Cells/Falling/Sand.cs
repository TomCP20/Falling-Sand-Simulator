namespace FallingSandSimulator;

public class Sand : Falling
{
    public Sand() : base(Colour.Noise((1, 1, 0), 0.1f)) { }
}