

using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class World
{
    private readonly int[,] state;

    public readonly Vector2i size;

    public World(int width, int height)
    {
        size = new Vector2i(width, height);
        state = new int[height, width];
        state[0, 0] = 1;
        state[0, 1] = 2;
        state[1, 0] = 3;
    }

    public void Update()
    {
        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1); j++)
            {
                int cell = state[i, j];
            }
        }
    }


    public float[] toArray()
    {
        float[] array = new float[3 * size.X * size.Y];

        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1); j++)
            {
                int index = i * state.GetLength(1) + j;
                int s = state[i, j];
                Vector3 col;
                if (s == 0)
                {
                    col = new Vector3(0, 0, 0);
                }
                else if (s == 1)
                {
                    col = new Vector3(1, 0, 0);
                }
                else if (s == 2)
                {
                    col = new Vector3(0, 1, 0);
                }
                else
                {
                    col = new Vector3(0, 0, 1);
                }
                for (int k = 0; k < 3; k++)
                {
                    array[index * 3 + k] = col[k];
                }
            }
        }
        return array;
    }
}