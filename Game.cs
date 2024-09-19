using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FallingSandSimulator;

public class Game : GameWindow
{
    private readonly QuadMesh quad = new();

    private readonly Shader shader = new();

    private readonly Shader screenShader = new();

    private readonly Texture texture;

    private readonly World world;

    private readonly Brush brush;

    private bool playing = true;

    private int framebuffer;

    private int textureColorbuffer;

    private int rbo;


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

        WindowState = WindowState.Maximized;

        GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        GL.Disable(EnableCap.DepthTest);

        quad.SetUp();

        shader.SetUp("Shaders/shader.vert", "Shaders/shader.frag");
        screenShader.SetUp("Shaders/screenShader.vert", "Shaders/screenShader.frag");

        screenShader.SetInt("screenTexture", 0);

        framebuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

        textureColorbuffer = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureColorbuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, FramebufferSize.X, FramebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureColorbuffer, 0);

        rbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, FramebufferSize.X, FramebufferSize.Y); // use a single renderbuffer object for both a depth AND stencil buffer.
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo); // now actually attach it
        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            Console.WriteLine("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        texture.SetUp();

        
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();
        texture.Use(TextureUnit.Texture0);
        quad.Draw();

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.Disable(EnableCap.DepthTest);
        GL.ClearColor(1, 1, 1, 1);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        screenShader.Use();
        GL.BindTexture(TextureTarget.Texture2D, textureColorbuffer);
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
                WindowState = WindowState.Maximized;
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

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);

        GL.BindTexture(TextureTarget.Texture2D, textureColorbuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, e.Width, e.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, e.Width, e.Height);
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