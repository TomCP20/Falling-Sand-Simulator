namespace FallingSandSimulator;

public class RainbowSand(int x, int y) : Powder(CellType.RainbowSand, x, y)
{
    static float hue = 0;

    public static (float, float, float) GetColour()
    {
        hue %= 1;
        (float, float, float) col = Colour.HueToRgb(hue);
        hue += 1f/3600f;
        return col;
    }
}