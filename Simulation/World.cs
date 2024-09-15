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
                    state[y, x]?.Update(this, x, y);
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

    public void SpawnCell(int x, int y, CellType spawnType)
    {
        if (InBounds(x, y))
        {
            state[y, x] = spawnType.NewCell();
        }
    }

    public void DeleteCell(int x, int y)
    {
        if (InBounds(x, y))
        {
            state[y, x] = null;
        }
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
        (float, float, float)[] UIcols = [Colour.DarkGrey, Colour.Blue, Colour.Yellow, Colour.White, Colour.Grey, Colour.SmokeGrey, Colour.Brown, Colour.Vermilion, Colour.Green, Colour.Grey];
        for (int y = height; y < height + UiHeight; y++)
        {
            for (int x = 0; x < width; x++)
            {
                (float, float, float) col;
                if (x/UiHeight < UIcols.Length)
                {
                    if (x % UiHeight == 0 || x % UiHeight == UiHeight - 1 || y == height || y == height + UiHeight - 1)
                    {
                        if (x/UiHeight == (int)brush.spawnType)
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
                        col = UIcols[x/UiHeight];
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

    public bool AttemptMoves(int x, int y, (int, int)[] deltas, int dir)
    {
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (IsEmpty(newx, newy))
            {
                MoveTo(x, y, newx, newy);
                return true;
            }
        }
        return false;
    }

    public bool AttemptDisplacements(int x, int y, (int, int)[] deltas, int dir)
    {
        foreach ((int dx, int dy) in deltas)
        {
            int newx = x + dx * dir;
            int newy = y + dy;
            if (IsDisplaceable(newx, newy))
            {
                Swap(x, y, newx, newy);
                return true;
            }
        }
        return false;
    }

    public IEnumerable<(int, int)> GetNeighbors(int x, int y)
    {
        (int, int)[] deltas = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];
        foreach ((int dx, int dy) in deltas)
        {
            if (InBounds(x + dx, y + dy))
            {
                yield return (x + dx, y + dy);
            }
        }
    }
}