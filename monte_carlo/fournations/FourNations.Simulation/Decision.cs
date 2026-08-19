namespace FourNations.Simulation;

/// <summary>
///
/// Eventually: 
/// public sealed record Decision(
///     AgentAction Action,
///     string Reason,
///     double Confidence);
/// </summary>
/// <param name="Action"></param>
/// <param name="Reason"></param>
/// <param name="Confidence"></param>
public sealed record Decision(AgentAction Action)
{
    private string Reason;
    private double Confidence;

    public override string ToString()
        => $"Action={Action}";
}