namespace FallingSandSimulator;

public class Wood : Solid
{
    public Wood() : base(Colour.Noise((0.54f, 0.27f, 0.07f), 0.02f)) { }

    public static readonly float burnChance = 0.02f;
}