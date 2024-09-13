namespace FallingSandSimulator;

public class Confetti: Falling
{
    public Confetti() : base(Colour.Static())
    {
        deltas1 = [(-1, -1), (1, -1), (0, -1)];
    }
}
