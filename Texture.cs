using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace FallingSandSimulator;

public class Texture
{
    public readonly int Handle;

    public static Texture setupTexture(World world)
    {
        int handle = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, world.width, world.height, 0, PixelFormat.Rgb, PixelType.Float, nint.Zero);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        return new Texture(handle);
    }

    public Texture(int glHandle)
    {
        Handle = glHandle;
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void update(World world, int mouseX, int mouseY, int brushSize, bool showUI)
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, world.width, world.height, 0, PixelFormat.Rgb, PixelType.Float, world.ToArray(mouseX, mouseY, brushSize, showUI));
    }
}