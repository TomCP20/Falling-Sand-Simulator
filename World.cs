using System.Diagnostics;
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

    private static Random rand = new();

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
        foreach (int x in ShuffleXs())
        {
            for (int y = 0; y < height; y++)
            {
                if (!stepped[y, x])
                {
                    state[y, x]?.Update(this, x, y);
                }
            }
        }
    }

    private int[] ShuffleXs()
    {
        int[] x = Enumerable.Range(0, width).ToArray();
        int n = width;
        while (n > 1)
        {
            int k = rand.Next(n--);
            (x[k], x[n]) = (x[n], x[k]);
        }
        return x;
    }

    public void Swap(int x1, int y1, int x2, int y2)
    {
        (state[y2, x2], state[y1, x1]) = (state[y1, x1], state[y2, x2]);
    }

    public void MoveTo(int x1, int y1, int x2, int y2)
    {
        Debug.Assert(state[y2, x2] == null);
        state[y2, x2] = state[y1, x1];
        state[y1, x1] = null;
        SetStepped(x2, y2);
    }

    public void SetStepped(int x, int y)
    {
        stepped[y, x] = true;
    }

    public void Clear()
    {
        state = state = new Cell[height, width];
    }

    public void SpawnCell<T>(int x, int y) where T : Cell?, new()
    {
        if (IsEmpty(x, y))
        {
            state[y, x] = new T();
        }
    }

    public void SpawnMultipleCells<T>(int x, int y, int radius) where T : Cell, new()
    {
        for (int yi = y-radius; yi <= y+radius; yi++)
        {
            for (int xi = x-radius; xi <= x+radius; xi++)
            {
                SpawnCell<T>(xi, yi);
            }
        }
    }

    public void EraseMultipleCells(int x, int y, int radius)
    {
        for (int yi = y-radius; yi <= y+radius; yi++)
        {
            for (int xi = x-radius; xi <= x+radius; xi++)
            {
                if (InBounds(xi, yi))
                {
                    state[yi, xi] = null;
                }
            }
        }
    }

    public bool IsEmpty(int x, int y)
    {
        return InBounds(x, y) && state[y, x] == null;
    }

    public bool IsDisplaceable(int x, int y)
    {
        return InBounds(x, y) && (state[y, x] is Water) && !stepped[y, x];
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
                    col = cell.colour;
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