using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Brush
{
    public int size = 15;

    public CellType spawnType = CellType.Sand;

    public Vector2i Pos = new Vector2i(-1, -1);

    private Vector2i screenSize;
    private Vector2i worldSize;

    public Brush(Vector2i screenSize, Vector2i worldSize) 
    {
        this.screenSize = screenSize;
        this.worldSize = worldSize;
    }


    public void Update(KeyboardState KeyboardState, MouseState MouseState, Vector2 MousePosition)
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

        size += (int)MouseState.ScrollDelta.Y;

        size = Math.Clamp(size, 0, 50);

        Pos.X = (int)Math.Floor(MousePosition.X / screenSize.X * worldSize.X);
        Pos.Y = (int)Math.Ceiling((1 - MousePosition.Y / screenSize.Y) * worldSize.Y) - 1;
    }

}