namespace FourNations.Simulation;

public abstract record AgentAction
{
    public sealed record Stay : AgentAction;

    public sealed record MoveTo(Position Destination)
        : AgentAction;

    public override string ToString()
        => this switch
        {
            Stay =>
                "Stay",

            MoveTo move =>
                $"MoveTo {move.Destination}",

            _ =>
                GetType().Name
        };
}