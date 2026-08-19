namespace FourNations.Simulation;

public sealed class Nation(string name)
{
    public string Name { get; } = name;

    public List<Agent> Agents { get; } = [];

    public override string ToString()
        => $"{Name} Agents={Agents.Count}";
}