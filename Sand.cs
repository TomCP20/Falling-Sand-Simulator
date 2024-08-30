using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Sand : Cell
{

    public Sand() : base(new Vector3(1, 1, 0)) { }

    public override void Update(World world, Cell?[,] nextState, int x, int y)
    {
        if (world.IsEmpty(x, y-1))
        {
            nextState[y, x] = null;
            nextState[y - 1, x] = this;
        }
        else
        {
            nextState[y, x] = this;
        }
    }
}