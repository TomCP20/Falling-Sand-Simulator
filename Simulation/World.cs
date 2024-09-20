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
        for (int y = 0; y < height; y++)
        {
            foreach (int x in ShuffleXs())
            {
                if (!stepped[y, x])
                {
                    Cell? cell = state[y, x];
                    if (cell != null)
                    {
                        if (!cell.Update(this))
                        {
                            stepped[y, x] = true;
                        }
                    }

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

    public bool InBounds(int x, int y)
    {
        return 0 <= x && x < width && 0 <= y && y < height;
    }
}