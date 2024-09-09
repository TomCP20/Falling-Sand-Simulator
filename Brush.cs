using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Brush(int screenWidth, int screenHeight, int worldWidth, int worldHeight)
{
    public int size = 15;

    public CellType spawnType = CellType.Sand;

    public int posX;

    public int posY;


    public void Update(KeyboardState KeyboardState, int ScrollDelta, float mouseX, float mouseY)
    {
        if (KeyboardState.IsKeyPressed(Keys.D0))
        {
            spawnType = CellType.Empty;
        }
        if (KeyboardState.IsKeyPressed(Keys.D1))
        {
            spawnType = CellType.Water;
        }
        if (KeyboardState.IsKeyPressed(Keys.D2))
        {
            spawnType = CellType.Sand;
        }
        if (KeyboardState.IsKeyPressed(Keys.D3))
        {
            spawnType = CellType.RainbowSand;
        }
        if (KeyboardState.IsKeyPressed(Keys.D4))
        {
            spawnType = CellType.Stone;
        }

        size = Math.Clamp(size + ScrollDelta, 0, 50);

        posX = (int)Math.Floor(mouseX / screenWidth * worldWidth);
        posY = (int)Math.Ceiling((1 - mouseY / screenHeight) * worldHeight) - 1;
    }

    public bool OnBorder(int x, int y)
    {
        return size == Math.Max(Math.Abs(y - posY), Math.Abs(x - posX));
    }

    public IEnumerable<(int, int)> getBrushCoords()
    {
        for (int yi = posY - size; yi <= posY + size; yi++)
        {
            for (int xi = posX - size; xi <= posX + size; xi++)
            {
                yield return (xi, yi);
            }
        }
    }

}