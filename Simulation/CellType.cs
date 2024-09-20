namespace FallingSandSimulator;

public enum CellType
{
    Empty,
    Water,
    Sand,
    RainbowSand,
    Brick,
    Smoke,
    Wood,
    Fire,
    Acid,
    Confetti,
    Titanium,
    Virus,
}

public static class CellTypeExtension
{
    public static Cell? NewCell(this CellType spawnType, int x, int y)
    {
        return spawnType switch
        {
            CellType.Empty => null,
            CellType.Water => new Water(x, y),
            CellType.Sand => new Sand(x, y),
            CellType.RainbowSand => new RainbowSand(x, y),
            CellType.Brick => new Brick(x, y),
            CellType.Smoke => new Smoke(x, y),
            CellType.Wood => new Wood(x, y),
            CellType.Fire => new Fire(x, y),
            CellType.Acid => new Acid(x, y),
            CellType.Confetti => new Confetti(x, y),
            CellType.Titanium => new Titanium(x, y),
            CellType.Virus => new Virus(x, y),
            _ => throw new Exception($"Case {spawnType} not found."),
        };
    }

    public static (float, float, float) GetCol(this CellType spawnType, int x, int y, int size)
    {
        return spawnType switch
        {
            CellType.Empty => Colour.DarkGrey,
            CellType.Water => Colour.Blue,
            CellType.Sand => Colour.Noise(Colour.Yellow, 0.1f),
            CellType.RainbowSand => Colour.HueToRgb(x/(float)size),
            CellType.Brick => Colour.BrickPattern(x, y),
            CellType.Smoke => Colour.SmokeGrey,
            CellType.Wood => Colour.Noise(Colour.Brown, 0.02f),
            CellType.Fire => Colour.RandomMix(Colour.Red, Colour.Yellow),
            CellType.Acid => Colour.Green,
            CellType.Confetti => Colour.Static(),
            CellType.Titanium => Colour.White,
            CellType.Virus => Colour.Noise(Colour.Purple, 0.4f),
            _ => throw new Exception($"Case {spawnType} not found."),
        };
    }

    public static bool Glows(this CellType spawnType)
    {
        return spawnType == CellType.Fire || spawnType == CellType.Acid || spawnType == CellType.Virus;
    }
}