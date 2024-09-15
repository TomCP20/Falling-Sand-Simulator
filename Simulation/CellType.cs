namespace FallingSandSimulator;

public enum CellType
{
    Empty,
    Water,
    Sand,
    RainbowSand,
    Stone,
    Smoke,
    Wood,
    Fire,
    Acid,
    Confetti,
}

public static class CellTypeExtension
{
    public static Cell? NewCell(this CellType spawnType)
    {
        return spawnType switch
        {
            CellType.Empty => null,
            CellType.Water => new Water(),
            CellType.Sand => new Sand(),
            CellType.RainbowSand => new RainbowSand(),
            CellType.Stone => new Stone(),
            CellType.Smoke => new Smoke(),
            CellType.Wood => new Wood(),
            CellType.Fire => new Fire(),
            CellType.Acid => new Acid(),
            CellType.Confetti => new Confetti(),
            _ => throw new Exception($"Case {spawnType} not found."),
        };
    }

    public static (float, float, float) GetCol(this CellType spawnType, int x, int y, int size)
    {
        return spawnType switch
        {
            CellType.Empty => Colour.DarkGrey,
            CellType.Water => Colour.Blue,
            CellType.Sand => Colour.Yellow,
            CellType.RainbowSand => Colour.HueToRgb(x/(float)size),
            CellType.Stone => Colour.Grey,
            CellType.Smoke => Colour.SmokeGrey,
            CellType.Wood => Colour.Brown,
            CellType.Fire => Colour.Vermilion,
            CellType.Acid => Colour.Green,
            CellType.Confetti => Colour.UIStatic(x, y, size),
            _ => throw new Exception($"Case {spawnType} not found."),
        };
    }
}