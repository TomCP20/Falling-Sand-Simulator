using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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

    private readonly Brush brush;


    private bool playing = true;

    private bool showUI = true;


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        brush = new(Size, new Vector2i(640, 480));
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        quad = new QuadMesh();

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        texture = Texture.setupTexture(world.width, world.height);
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
        texture.update(world.ToArray(brush.Pos.X, brush.Pos.Y, brush.size, showUI));
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