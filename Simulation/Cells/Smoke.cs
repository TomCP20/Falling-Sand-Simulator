namespace FallingSandSimulator;

public class Smoke : Cell
{
    public Smoke() : base((0.51f, 0.53f, 0.51f)) { }

    private static bool Decay(float decayChance)
    {
        return rand.NextSingle() <= decayChance;
    }

    public override void Update(World world, int x, int y)
    {
        if (Decay(0.01f))
        {
            world.DeleteCell(x, y);
            return;
        }
        (int, int)[] deltas = [(0, 1), (-1, 1), (1, 1), (-1, 0), (1, 0)];
        if(AttemptMoves(world, x, y, deltas))
        {
            return;
        }
        world.SetStepped(x, y);
    }
}