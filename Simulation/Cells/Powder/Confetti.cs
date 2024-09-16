namespace FallingSandSimulator;

public class Confetti: Powder
{
    public Confetti(int x, int y) : base(Colour.Static(), x, y)
    {
        deltas1 = [(-1, -1), (1, -1), (0, -1)];
    }
}
