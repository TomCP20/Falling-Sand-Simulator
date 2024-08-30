using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class Stone : Cell
{

    public Stone() : base(new Vector3(0.5f, 0.5f, 0.5f)) { }

    public override void Update(World world, Cell?[,] nextState, int x, int y)
    {
        nextState[y, x] = this;
    }
}