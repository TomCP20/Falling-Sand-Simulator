namespace FallingSandSimulator;

public class Wood : Solid
{
    public Wood() : base((0.54f, 0.27f, 0.07f)) { }

    public static readonly float burnChance = 0.02f;
}