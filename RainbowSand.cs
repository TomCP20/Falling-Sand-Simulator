using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class RainbowSand : Sand
{
    static float hue = 0;

    private static (float, float, float) GetColour()
    {
        hue %= 1;
        (float, float, float) col = Colour.HueToRgb(hue);
        hue += 1.0f/360.0f;
        return col;
    }

    public RainbowSand() : base(GetColour()) { }

}