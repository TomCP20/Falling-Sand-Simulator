using System.Diagnostics;

namespace FallingSandSimulator;

public class World(int width, int height, int UiHeight)
{
    private Cell?[,] state = new Cell[height, width];

    private bool[,] stepped = new bool[height, width];


    private static readonly Random rand = new();

    public void Update()
    {
        stepped = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            foreach (int x in ShuffleXs())
            {
                if (!stepped[y, x])
                {
                    state[y, x]?.Update(this);
                }
            }
        }
    }

    public Cell? GetCell(int x, int y)
    {
        return state[y, x];
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

    public void Swap(Cell? cell1, Cell? cell2)
    {
        Debug.Assert(cell1 != null);
        Debug.Assert(cell2 != null);
        (state[cell2.y, cell2.x], state[cell1.y, cell1.x]) = (cell1, cell2);
        (cell1.x, cell2.x) = (cell2.x, cell1.x);
        (cell1.y, cell2.y) = (cell2.y, cell1.y);
        SetStepped(cell1);
        SetStepped(cell2);
    }

    public void MoveTo(Cell? cell, int x, int y)
    {
        Cell? b = state[y, x];
        Debug.Assert(cell != null);
        Debug.Assert(b == null);
        state[y, x] = cell;
        state[cell.y, cell.x] = null;
        cell.x = x;
        cell.y = y;
        SetStepped(cell);
    }

    public void SetStepped(Cell cell)
    {
        stepped[cell.y, cell.x] = true;
    }

    public void Clear()
    {
        state = new Cell[height, width];
    }

    public void SpawnCell(int x, int y, CellType spawnType)
    {
        if (InBounds(x, y))
        {
            state[y, x] = spawnType.NewCell(x, y);
        }
    }

    public void DeleteCell(Cell cell)
    {
        state[cell.y, cell.x] = null;
    }

    public bool IsEmpty(int x, int y)
    {
        return InBounds(x, y) && state[y, x] == null;
    }

    public bool IsDisplaceable(int x, int y)
    {
        if (InBounds(x, y))
        {
            Cell? cell = state[y, x];
            return cell == null || cell.displaceable;
        }
        return false;
    }

    public bool InBounds(int x, int y)
    {
        return 0 <= x && x < width && 0 <= y && y < height;
    }


    public float[] ToArray(Brush brush)
    {
        float[] array = new float[3 * width * (height + UiHeight)];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                (float, float, float) col;
                if (brush.show && brush.OnBorder(x, y) && brush.InBounds())
                {
                    col = Colour.White;
                }
                else
                {
                    Cell? cell = state[y, x];
                    if (cell == null)
                    {
                        col = Colour.DarkGrey;
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
        for (int y = height; y < height + UiHeight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                (float, float, float) col;
                if (Enum.IsDefined(typeof(CellType), x/UiHeight))
                {
                    CellType type = (CellType)(x/UiHeight);
                    if (x % UiHeight == 0 || x % UiHeight == UiHeight - 1 || y == height || y == height + UiHeight - 1)
                    {
                        if (type == brush.spawnType)
                        {
                            col = Colour.White;
                        }
                        else
                        {
                            col = Colour.Black;
                        }  
                    }
                    else
                    {
                        col = type.GetCol((x % UiHeight)-1, y-height-1, UiHeight-2);
                    }
                }
                else
                {
                    col = (0, 0, 0);
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