using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Brush(int screenWidth, int screenHeight, int worldWidth, int worldHeight, int UiHeight)
{
    public int size = 15;

    public CellType spawnType = CellType.Sand;

    public int posX;

    public int posY;

    public bool show = true;


    public void Update(int ScrollDelta, float mouseX, float mouseY)
    {
        size = Math.Clamp(size + ScrollDelta, 0, 50);

        posX = (int)Math.Floor(mouseX / screenWidth * worldWidth);
        posY = (int)Math.Ceiling((1 - mouseY / screenHeight) * (worldHeight + UiHeight)) - 1;
    }

    public bool OnBorder(int x, int y)
    {
        return size == Math.Max(Math.Abs(y - posY), Math.Abs(x - posX));
    }

    public IEnumerable<(int, int)> GetBrushCoords()
    {
        for (int yi = posY - size; yi <= posY + size; yi++)
        {
            for (int xi = posX - size; xi <= posX + size; xi++)
            {
                yield return (xi, yi);
            }
        }
    }
    public void Draw(World world)
    {
        switch (spawnType)
        {
            case CellType.Empty:
                EraseCells(world);
                break;
            case CellType.Water:
                SpawnCells<Water>(world);
                break;
            case CellType.Sand:
                SpawnCells<Sand>(world);
                break;
            case CellType.RainbowSand:
                SpawnCells<RainbowSand>(world);
                break;
            case CellType.Stone:
                SpawnCells<Stone>(world);
                break;
            case CellType.Smoke:
                SpawnCells<Smoke>(world);
                break;
            case CellType.Wood:
                SpawnCells<Wood>(world);
                break;
            case CellType.Fire:
                SpawnCells<Fire>(world);
                break;
            case CellType.Acid:
                SpawnCells<Acid>(world);
                break;
            case CellType.Confetti:
                SpawnCells<Confetti>(world);
                break;
            default:
                throw new Exception($"Case {spawnType} not found.");
        }
    }

    public void SpawnCells<T>(World world) where T : Cell, new()
    {
        foreach ((int x, int y) in GetBrushCoords())
        {
            if (world.IsEmpty(x, y))
            {
                world.SpawnCell<T>(x, y);
            }
        }
    }

    public void EraseCells(World world)
    {
        foreach ((int x, int y) in GetBrushCoords())
        {
            world.DeleteCell(x, y);
        }
    }

    public bool InBounds()
    {
        return 0 <= posX && posX < worldWidth && 0 <= posY && posY < worldHeight;
    }

    public bool OnUI()
    {
        return 0 <= posX && posX < worldWidth && worldHeight <= posY && posY < worldHeight + UiHeight;
    }

    public void SetType()
    {
        if (posX/UiHeight < Enum.GetValues(typeof(CellType)).Length)
        {
            spawnType = (CellType)(posX/UiHeight);
        }
        
    }

}