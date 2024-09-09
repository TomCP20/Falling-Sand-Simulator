using System.Diagnostics;

namespace FallingSandSimulator;

public class World(int width, int height)
{
    private Cell?[,] state = new Cell[height, width];

    private bool[,] stepped = new bool[height, width];


    private static readonly Random rand = new();

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
        SetStepped(x1, y1);
        SetStepped(x2, y2);
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

    public void DrawBrush(Brush brush)
    {
        switch (brush.spawnType)
        {
            case CellType.Empty:
                EraseMultipleCells(brush);
                break;
            case CellType.Water:
                SpawnMultipleCells<Water>(brush);
                break;
            case CellType.Sand:
                SpawnMultipleCells<Sand>(brush);
                break;
            case CellType.RainbowSand:
                SpawnMultipleCells<RainbowSand>(brush);
                break;
            case CellType.Stone:
                SpawnMultipleCells<Stone>(brush);
                break;
        }
    }

    public void SpawnMultipleCells<T>(Brush brush) where T : Cell, new()
    {
        foreach ((int x, int y) in brush.getBrushCoords())
        {
            SpawnCell<T>(x, y);
        }
    }

    public void EraseMultipleCells(Brush brush)
    {
        foreach ((int x, int y) in brush.getBrushCoords())
        {
            if (InBounds(x, y))
            {
                state[y, x] = null;
            }
        }
    }

    public bool IsEmpty(int x, int y)
    {
        return InBounds(x, y) && state[y, x] == null;
    }

    public bool IsDisplaceable(int x, int y)
    {
        return InBounds(x, y) && (state[y, x] is Water);
    }

    public bool InBounds(int x, int y)
    {
        return 0 <= x && x < width && 0 <= y && y < height;
    }


    public float[] ToArray(Brush brush, bool showUI)
    {
        float[] array = new float[3 * width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                (float, float, float) col;
                if (showUI && brush.OnBorder(x, y))
                {
                    col = (1, 1, 1);
                }
                else
                {
                    Cell? cell = state[y, x];
                    if (cell == null)
                    {
                        col = (0, 0, 0);
                    }
                    else
                    {
                        col = cell.colour;
                    }
                }
                int index = y * width + x;

                array[index * 3 + 0] = col.Item1;
                array[index * 3 + 1] = col.Item2;
                array[index * 3 + 2] = col.Item3;
            }
        }
        return array;
    }
}