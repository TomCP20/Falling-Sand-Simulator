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
        foreach ((int x, int y) in GetBrushCoords())
        {
            if (spawnType == CellType.Empty || world.IsEmpty(x, y))
            {
                world.SpawnCell(x, y, spawnType);
            }
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