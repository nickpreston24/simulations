namespace FourNations.Simulation;

/// <summary>
/// Observation
/// This is an important abstraction.
///
///     The agent shouldn't necessarily receive the entire World.
///
///     Eventually that's going to matter a lot.
/// </summary>
/// <param name="Tick"></param>
/// <param name="Position"></param>
public sealed record Observation(
    long Tick,
    Position Position,
    Position Target)
{
    public int DistanceToTarget
        => Position.DistanceTo(Target);

    public override string ToString()
        => $"Position={Position} " +
           $"Target={Target} " +
           $"Distance={DistanceToTarget}";
}