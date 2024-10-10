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

    private readonly Shader blur = new();

    private readonly Texture texture;

    private readonly World world;

    private readonly Brush brush;

    private bool playing = true;

    private int framebuffer;

    private readonly int[] textureColorbuffers = new int[2];

    private readonly int[] pingpongFBO = new int[2];
    private readonly int[] pingpongColorbuffers = new int[2];

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

        texture.SetUp();

        shader.SetUp("Shaders/shader.vert", "Shaders/shader.frag");
        screenShader.SetUp("Shaders/screenShader.vert", "Shaders/screenShader.frag");
        blur.SetUp("Shaders/blur.vert", "Shaders/blur.frag");

        framebuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

        GL.GenTextures(2, textureColorbuffers);
        for (int i = 0; i < 2; i++)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureColorbuffers[i]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, FramebufferSize.X, FramebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, textureColorbuffers[i], 0);
        }

        DrawBuffersEnum[] attachments = [DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1];
        GL.DrawBuffers(2, attachments);

        rbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, FramebufferSize.X, FramebufferSize.Y); // use a single renderbuffer object for both a depth AND stencil buffer.
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo); // now actually attach it
        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            Console.WriteLine("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        // ping-pong-framebuffer for blurring
        
        GL.GenFramebuffers(2, pingpongFBO);
        GL.GenTextures(2, pingpongColorbuffers);
        for (int i = 0; i < 2; i++)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingpongFBO[i]);
            GL.BindTexture(TextureTarget.Texture2D, pingpongColorbuffers[i]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, FramebufferSize.X, FramebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge); // we clamp to the edge as the blur filter would otherwise sample repeated texture values!
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, pingpongColorbuffers[i], 0);
            // also check if framebuffers are complete (no need for depth buffer)
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("ERROR::FRAMEBUFFER:: Framebuffer is not complete!");
            }
        }

        blur.Use();
        blur.SetInt("image", 0);

        screenShader.Use();
        screenShader.SetInt("scene", 0);
        screenShader.SetInt("bloomBlur", 1);
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

        int horizontal = 1;
        bool first_iteration = true;
        int amount = 10;
        blur.Use();
        for (int i = 0; i < amount; i++)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingpongFBO[horizontal]);
            blur.SetInt("horizontal", horizontal);
            GL.BindTexture(TextureTarget.Texture2D, first_iteration ? textureColorbuffers[1] : pingpongColorbuffers[1-horizontal]);  // bind texture of other framebuffer (or scene if first iteration)
            quad.Draw();
            horizontal = 1-horizontal;
            if (first_iteration)
                first_iteration = false;
        }
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        screenShader.Use();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, textureColorbuffers[0]);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, pingpongColorbuffers[1-horizontal]);
         
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
            if (IsFullscreen)
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

        for (int i = 0; i < 2; i++)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureColorbuffers[i]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, e.Width, e.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
        }

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, e.Width, e.Height);

        for (int i = 0; i < 2; i++)
        {
            GL.BindTexture(TextureTarget.Texture2D, pingpongColorbuffers[i]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, FramebufferSize.X, FramebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
        }
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