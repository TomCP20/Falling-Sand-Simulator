using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Game : GameWindow
{
    private QuadMesh? quad;

    private Shader? shader;

    private Texture? texture;

    private readonly World world = new World(640, 480);

    private CellType spawnType = CellType.Sand;

    private readonly int brushSize = 8;


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        quad = new QuadMesh();

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        texture = Texture.fromWorld(world);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        Debug.Assert(quad != null);
        Debug.Assert(shader != null);
        Debug.Assert(texture != null);
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        texture.Use(TextureUnit.Texture0);

        quad.Draw();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        Debug.Assert(texture != null);
        Debug.Assert(world != null);
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (KeyboardState.IsKeyPressed(Keys.C))
        {
            world.Clear();
        }

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

        if (IsMouseButtonDown(MouseButton.Left))
        {
            int x = (int)Math.Floor(MousePosition.X / Size.X * world.width);
            int y = (int)Math.Ceiling((1 - MousePosition.Y / Size.Y) * world.height) - 1;
            switch (spawnType)
            {
                case CellType.Empty:
                    world.EraseMultipleCells(x, y, brushSize);
                    break;
                case CellType.Water:
                    world.SpawnMultipleCells<Water>(x, y, brushSize);
                    break;
                case CellType.Sand:
                    world.SpawnMultipleCells<Sand>(x, y, brushSize);
                    break;
                case CellType.RainbowSand:
                    world.SpawnMultipleCells<RainbowSand>(x, y, brushSize);
                    break;
                case CellType.Stone:
                    world.SpawnMultipleCells<Stone>(x, y, brushSize);
                    break;
            }
        }

        world.Update();
        texture.update(world);
    }


    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    protected override void OnUnload()
    {
        Debug.Assert(quad != null);
        Debug.Assert(shader != null);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        quad.Delete();
        GL.DeleteProgram(shader.Handle);
        base.OnUnload();
    }

}