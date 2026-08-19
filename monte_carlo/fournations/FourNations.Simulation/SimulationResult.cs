namespace FourNations.Simulation;
public sealed record SimulationResult(
    Guid RunId,
    int Seed,
    long Ticks,
    bool Success,
    IReadOnlyList<Agent> Agents)
{
    public override string ToString()
    {
        var status = Success
            ? "SUCCESS"
            : "FAILED";

        return
            $"Run={RunId} " +
            $"Seed={Seed} " +
            $"Status={status} " +
            $"Ticks={Ticks} " +
            $"Agents={Agents.Count}";
    }
}