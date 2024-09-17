using OpenTK.Graphics.OpenGL4;
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

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, int worldWidth, int worldHeight, int UiHeight) : base(gameWindowSettings, nativeWindowSettings)
    {
        world = new(worldWidth, worldHeight);
        brush = new(worldWidth, worldHeight, UiHeight);
        texture = new(worldWidth, worldHeight, UiHeight);
        texture.SetUpUI();
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

        brush.Update((int)MouseState.ScrollDelta.Y);
        brush.posX = (int)Math.Floor(MousePosition.X / FramebufferSize.X * texture.Width);
        brush.posY = (int)Math.Ceiling((1 - MousePosition.Y / FramebufferSize.Y) * texture.Height) - 1;

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (KeyboardState.IsKeyPressed(Keys.Space))
        {
            playing = !playing;
        }

        if (KeyboardState.IsKeyPressed(Keys.C))
        {
            world.Clear();
        }

        if (KeyboardState.IsKeyPressed(Keys.H))
        {
            brush.show = !brush.show;
        }

        if (KeyboardState.IsKeyPressed(Keys.F))
        {
            if(IsFullscreen)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Fullscreen;
            }
        }

        if (playing || KeyboardState.IsKeyPressed(Keys.Right))
        {
            world.Update();
        }

        if (IsMouseButtonDown(MouseButton.Left) && brush.InBounds())
        {
            brush.Draw(world);
        }

        if (IsMouseButtonReleased(MouseButton.Left) && brush.OnUI())
        {
            brush.SetType();
        }

        texture.Update(world, brush);
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs args)
    {
        base.OnFramebufferResize(args);
        GL.Viewport(0, 0, args.Width, args.Height);
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