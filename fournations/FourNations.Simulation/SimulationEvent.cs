namespace FourNations.Simulation;

public sealed record SimulationEvent(
    Guid RunId,
    int Seed,
    long Tick,
    Guid AgentId,
    string AgentName,
    string EventType,
    string Description)
{
    public override string ToString()
        => $"Run={RunId} " +
           $"Seed={Seed} " +
           $"Tick={Tick:000000} " +
           $"Agent={AgentName} " +
           $"Event={EventType} " +
           $"{Description}";
}