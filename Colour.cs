using OpenTK.Mathematics;

namespace FallingSandSimulator;

static class Colour
{
    private static readonly Random rand = new();
    public static (float, float, float) HueToRgb(float hue)
    {
        float r = Math.Abs(hue * 6.0f - 3.0f);
        float g = 2.0f - Math.Abs(hue * 6.0f - 2.0f);
        float b = 2.0f - Math.Abs(hue * 6.0f - 4.0f);
        return (r, g, b);
    }

    public static (float, float, float) Noise((float, float, float) colour, float variation)
    {
        colour.Item1 = Noise(colour.Item1, variation);
        colour.Item2 = Noise(colour.Item2, variation);
        colour.Item3 = Noise(colour.Item3, variation);
        return colour;
    }

    private static float Noise(float val, float variation)
    {
        val += (rand.NextSingle() * 2 - 1) * variation; //chages by -var to +var
        return Math.Clamp(val, 0, 1);
    }

    public static (float, float, float) GreyNoise(float val, float variation)
    {
        float scale = Noise(val, variation);
        return (scale, scale, scale);
    }
}