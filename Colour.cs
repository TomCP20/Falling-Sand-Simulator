using OpenTK.Mathematics;

namespace FallingSandSimulator;

static class Colour
{
    public static (float, float, float) HueToRgb(float hue)
    {
        float r = Math.Abs(hue * 6.0f - 3.0f);
        float g = 2.0f - Math.Abs(hue * 6.0f - 2.0f);
        float b = 2.0f - Math.Abs(hue * 6.0f - 4.0f);
        return (r, g, b);
    }
}