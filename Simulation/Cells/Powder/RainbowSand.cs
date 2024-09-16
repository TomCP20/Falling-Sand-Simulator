namespace FallingSandSimulator;

public class RainbowSand(int x, int y) : Powder(GetColour(), x, y)
{
    static float hue = 0;

    private static (float, float, float) GetColour()
    {
        hue %= 1;
        (float, float, float) col = Colour.HueToRgb(hue);
        hue += 1f/3600f;
        return col;
    }
}