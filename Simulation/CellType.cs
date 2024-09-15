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
        switch (spawnType)
        {
            case CellType.Empty:
                return null;
            case CellType.Water:
                return new Water();
            case CellType.Sand:
                return new Sand();
            case CellType.RainbowSand:
                return new RainbowSand();
            case CellType.Stone:
                return new Stone();
            case CellType.Smoke:
                return new Smoke();
            case CellType.Wood:
                return new Wood();
            case CellType.Fire:
                return new Fire();
            case CellType.Acid:
                return new Acid();
            case CellType.Confetti:
                return new Confetti();
            default:
                throw new Exception($"Case {spawnType} not found.");
        }
    }

    public static (float, float, float) GetCol(this CellType spawnType)
    {
        (float, float, float)[] UIcols = [Colour.DarkGrey, Colour.Blue, Colour.Yellow, Colour.White, Colour.Grey, Colour.SmokeGrey, Colour.Brown, Colour.Vermilion, Colour.Green, Colour.Grey];
        return UIcols[(int)spawnType];
    }
}