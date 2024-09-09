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
        for (int yi = brush.Pos.Y - brush.size; yi <= brush.Pos.Y + brush.size; yi++)
        {
            for (int xi = brush.Pos.X - brush.size; xi <= brush.Pos.X + brush.size; xi++)
            {
                SpawnCell<T>(xi, yi);
            }
        }
    }

    public void EraseMultipleCells(Brush brush)
    {
        for (int yi = brush.Pos.Y - brush.size; yi <= brush.Pos.Y + brush.size; yi++)
        {
            for (int xi = brush.Pos.X - brush.size; xi <= brush.Pos.X + brush.size; xi++)
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
        return InBounds(x, y) && (state[y, x] is Water);
    }

    public bool InBounds(int x, int y)
    {
        return 0 <= x && x < width && 0 <= y && y < height;
    }


    public float[] ToArray(int mouseX, int mouseY, int brushSize, bool showUI)
    {
        float[] array = new float[3 * width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 col;
                if (showUI && (((x == mouseX + brushSize || x == mouseX - brushSize) && y <= mouseY + brushSize && y >= mouseY - brushSize) || ((y == mouseY + brushSize || y == mouseY - brushSize) && x <= mouseX + brushSize && x >= mouseX - brushSize)))
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
                int index = y * width + x;
                for (int k = 0; k < 3; k++)
                {
                    array[index * 3 + k] = col[k];
                }
            }
        }
        return array;
    }
}