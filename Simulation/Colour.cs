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

    public static (float, float, float) RandomMix((float, float, float) col1, (float, float, float) col2)
    {
        return Mix(col1, col2, rand.NextSingle());
    }

    public static (float, float, float) Mix((float, float, float) col1, (float, float, float) col2, float ratio)
    {
        (float r1, float g1, float b1) = col1;
        (float r2, float g2, float b2) = col2;
        return (Mix(r1, r2, ratio), Mix(g1, g2, ratio), Mix(b1, b2, ratio));
    }

    public static float Mix(float val1, float val2, float ratio)
    {
        return val1 * ratio + val2 * (1 - ratio);
    }
}