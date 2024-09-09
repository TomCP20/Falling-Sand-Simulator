using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Game : GameWindow
{
    private readonly QuadMesh quad = new();

    private readonly Shader shader = new();

    private readonly Texture texture;

    private readonly World world;

    private readonly Brush brush;

    private bool playing = true;

    private bool showUI = true;


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Vector2i worldSize) : base(gameWindowSettings, nativeWindowSettings)
    {
        world = new(worldSize);
        brush = new(Size, worldSize);
        texture = new(worldSize);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        quad.SetUp();

        shader.SetUp("Shaders/shader.vert", "Shaders/shader.frag");

        texture.SetUp();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        texture.Use(TextureUnit.Texture0);

        quad.Draw();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        brush.Update(KeyboardState, MouseState, MousePosition);

        

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (KeyboardState.IsKeyPressed(Keys.Space))
        {
            playing = !playing;
        }

        if (KeyboardState.IsKeyPressed(Keys.H))
        {
            showUI = !showUI;
        }

        if (KeyboardState.IsKeyPressed(Keys.C))
        {
            world.Clear();
        }

        if (IsMouseButtonDown(MouseButton.Left))
        {
            world.DrawBrush(brush);
        }

        if (playing)
        {
            world.Update();
        }
        texture.update(world.ToArray(brush, showUI));
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        quad.Delete();
        GL.DeleteProgram(shader.Handle);
        base.OnUnload();
    }

}