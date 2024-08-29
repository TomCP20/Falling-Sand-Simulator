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

    private World? world;


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        quad = new QuadMesh();

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        world = new World(4, 5);

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
    base.OnUpdateFrame(args);

    if (KeyboardState.IsKeyDown(Keys.Escape))
    {
        Close();
    }


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