using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Brush(Vector2i screenSize, Vector2i worldSize)
{
    public int size = 15;

    public CellType spawnType = CellType.Sand;

    public Vector2i Pos;

    private Vector2i screenSize = screenSize;
    private Vector2i worldSize = worldSize;

    public void Update(KeyboardState KeyboardState, int ScrollDelta, Vector2 MousePosition)
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

        Pos.X = (int)Math.Floor(MousePosition.X / screenSize.X * worldSize.X);
        Pos.Y = (int)Math.Ceiling((1 - MousePosition.Y / screenSize.Y) * worldSize.Y) - 1;
    }

    public bool OnBorder(int x, int y)
    {
        return size == Math.Max(Math.Abs(y - Pos.Y), Math.Abs(x - Pos.X));
    }

    public IEnumerable<(int, int)> getBrushCoords()
    {
        for (int yi = Pos.Y - size; yi <= Pos.Y + size; yi++)
        {
            for (int xi = Pos.X - size; xi <= Pos.X + size; xi++)
            {
                yield return (xi, yi);
            }
        }
    }

}