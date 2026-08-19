namespace FourNations.Simulation;

public readonly record struct Position(int X, int Y)
{
    public int DistanceTo(Position other)
        => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

    public override string ToString()
        => $"({X},{Y})";
}