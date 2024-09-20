namespace FallingSandSimulator;

public abstract class Powder(CellType type, int x, int y) : Moving(type, x, y)
{
    protected (int, int)[] deltas1 = [(0, -1), (-1, -1), (1, -1)];
    protected (int, int)[] deltas2 = [(-1, -1), (1, -1), (0, -1)];
    public override void Update(World world)
    {
        deltas = InLiquid(world) ? deltas2 : deltas1;
        base.Update(world);
    }
}