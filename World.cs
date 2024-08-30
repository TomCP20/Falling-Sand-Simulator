using System.Security.Cryptography.X509Certificates;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class World
{
    private Cell?[,] state;

    private bool[,] stepped;

    public readonly int width;

    public readonly int height;

    public World(int width, int height)
    {
        this.width = width;
        this.height = height;
        state = new Cell[height, width];
        stepped = new bool[height, width];
    }

    public void Update()
    {
        stepped = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                state[y, x]?.Update(this, x, y);
            }
        }
    }

    public void Swap(int x1, int y1, int x2, int y2)
    {
        (state[y2, x2], state[y1, x1]) = (state[y1, x1], state[y2, x2]);
    }

    public void SetStepped(int x, int y)
    {
        stepped[y, x] = true;
    }

    public void Clear()
    {
        state = state = new Cell[height, width];
    }

    public void SpawnSand(int x, int y)
    {
        if (IsEmpty(x, y))
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


    public float[] ToArray()
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