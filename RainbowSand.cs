using OpenTK.Mathematics;

namespace FallingSandSimulator;

public class RainbowSand : Sand
{
    static float hue = 0;

    private static Vector3 GetColour()
    {
        hue %= 1;
        float r = Math.Abs(hue * 6.0f - 3.0f);
        float g = 2.0f - Math.Abs(hue * 6.0f - 2.0f);
        float b = 2.0f - Math.Abs(hue * 6.0f - 4.0f);
        hue += 1.0f/360.0f;
        return new Vector3(r,g,b);
    }

    public RainbowSand() : base(GetColour()) { }

}