namespace FourNations.Simulation;

public sealed class World
{
    public long Tick { get; private set; }

    public List<Nation> Nations { get; } = [];

    public IEnumerable<Agent> AllAgents()
        => Nations.SelectMany(x => x.Agents);

    public bool AllAgentsComplete
        => AllAgents().All(x => x.HasReachedTarget);

    public void Advance()
        => Tick++;

    public override string ToString()
        => $"Tick={Tick} " +
           $"Nations={Nations.Count} " +
           $"Agents={AllAgents().Count()}";
}