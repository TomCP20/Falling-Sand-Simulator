namespace FallingSandSimulator;

public class Brick(int x, int y) : Solid(Colour.BrickPattern(x, y), x, y) { }