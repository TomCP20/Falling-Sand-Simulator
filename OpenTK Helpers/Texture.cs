using OpenTK.Graphics.OpenGL4;

namespace FallingSandSimulator;

public class Texture(int worldWidth, int worldHeight, int UiHeight)
{
    public int Handle;

    public int Width
    {
        get => worldWidth;
    }

    public int Height
    {
        get => worldHeight + UiHeight;
    }

    private readonly float[] array = new float[3 * worldWidth * (worldHeight + UiHeight)];

    public void SetUp()
    {
        Handle = GL.GenTexture();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, Handle);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, worldWidth, worldHeight + UiHeight, 0, PixelFormat.Rgb, PixelType.Float, nint.Zero);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void UpdateArray(World world, Brush brush)
    {
        UpdateBody(world, brush);
        UpdateUI(brush);
    }
    public void UpdateBody(World world, Brush brush)
    {
        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                (float, float, float) col;
                if (brush.show && brush.OnBorder(x, y) && brush.InBounds())
                {
                    col = Colour.White;
                }
                else
                {
                    Cell? cell = world.GetCell(x, y);
                    if (cell == null)
                    {
                        col = Colour.DarkGrey;
                    }
                    else
                    {
                        col = cell.colour;
                    }
                }
                int index = y * worldWidth + x;

                array[index * 3 + 0] = col.Item1;
                array[index * 3 + 1] = col.Item2;
                array[index * 3 + 2] = col.Item3;
            }
        }
    }

    public void UpdateUI(Brush brush)
    {
        for (int y = worldHeight; y < worldHeight + UiHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            { 
                (float, float, float) col;
                if (Enum.IsDefined(typeof(CellType), x / UiHeight))
                {
                    if (x % UiHeight == 0 || x % UiHeight == UiHeight - 1 || y == worldHeight || y == worldHeight + UiHeight - 1)
                    {
                        if ((CellType)(x / UiHeight) == brush.spawnType)
                        {
                            col = Colour.White;
                        }
                        else
                        {
                            col = Colour.Black;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    col = Colour.Black;
                }
                int index = y * worldWidth + x;
                array[index * 3 + 0] = col.Item1;
                array[index * 3 + 1] = col.Item2;
                array[index * 3 + 2] = col.Item3;
            }
        }
    }

    public void SetUpUI()
    {
        for (int y = worldHeight; y < worldHeight + UiHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                (float, float, float) col;
                if (Enum.IsDefined(typeof(CellType), x / UiHeight))
                {
                    if (!(x % UiHeight == 0 || x % UiHeight == UiHeight - 1 || y == worldHeight || y == worldHeight + UiHeight - 1))
                    {
                        col = ((CellType)(x / UiHeight)).GetCol((x % UiHeight) - 1, y - worldHeight - 1, UiHeight - 2);
                        int index = y * worldWidth + x;
                        array[index * 3 + 0] = col.Item1;
                        array[index * 3 + 1] = col.Item2;
                        array[index * 3 + 2] = col.Item3;
                    }
                }

            }
        }
    }

    public void Update(World world, Brush brush)
    {
        UpdateArray(world, brush);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, worldWidth, worldHeight + UiHeight, 0, PixelFormat.Rgb, PixelType.Float, array);
    }
}