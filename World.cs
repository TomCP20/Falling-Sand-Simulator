using System.Security.Cryptography.X509Certificates;
using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class World
{
    private Cell?[,] state;

    public readonly int width;

    public readonly int height;

    public World(int width, int height)
    {
        this.width = width;
        this.height = height;
        state = new Cell[height, width];
        state[height - 1, 0] = new Sand();
    }

    public void Update()
    {
        Cell?[,] nextState = new Cell[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                state[y, x]?.Update(this, nextState, x, y);
            }
        }
        state = nextState;
    }

    public void SpawnSand(int x, int y)
    {
        if (InBounds(x, y))
        {
            state[y, x] = new Sand();
        }
    }

    public bool IsEmpty(int x, int y)
    {
        if (!InBounds(x, y))
        {
            return false;
        }
        return state[y, x] == null;
    }

    public bool InBounds(int x, int y)
    {
        return 0 <= x && x < width && 0 <= y && y < height;
    }


    public float[] toArray()
    {
        float[] array = new float[3 * width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                Cell? cell = state[y, x];
                Vector3 col;
                if (cell == null)
                {
                    col = new Vector3(0, 0, 0);
                }
                else
                {
                    col = cell.col;
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