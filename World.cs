using System.Diagnostics;
using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class World(Vector2i size)
{
    private Cell?[,] state = new Cell[size.Y, size.X];

    private bool[,] stepped = new bool[size.Y, size.X];

    public readonly Vector2i size = size;

    private static readonly Random rand = new();

    public void Update()
    {
        stepped = new bool[size.Y, size.X];
        foreach (int x in ShuffleXs())
        {
            for (int y = 0; y < size.Y; y++)
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
        int[] x = Enumerable.Range(0, size.X).ToArray();
        int n = size.X;
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
        state = state = new Cell[size.Y, size.X];
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
        return 0 <= x && x < size.X && 0 <= y && y < size.Y;
    }


    public float[] ToArray(Brush brush, bool showUI)
    {
        float[] array = new float[3 * size.X * size.Y];

        for (int y = 0; y < size.Y; y++)
        {
            for (int x = 0; x < size.X; x++)
            {
                Vector3 col;
                if (showUI && brush.OnBorder(x, y))
                {
                    col = new Vector3(1, 1, 1);
                }
                else
                {
                    Cell? cell = state[y, x];
                    if (cell == null)
                    {
                        col = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        col = cell.colour;
                    }
                }
                int index = y * size.X + x;
                for (int k = 0; k < 3; k++)
                {
                    array[index * 3 + k] = col[k];
                }
            }
        }
        return array;
    }
}