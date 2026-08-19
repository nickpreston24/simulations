namespace FourNations.Simulation;

public sealed class Simulation
{
    private readonly List<SimulationEvent> events = [];

    private readonly Random random;

    // Unique identity of this particular execution.
    // Deliberately NOT used for equivalence.
    public Guid RunId { get; } = Guid.NewGuid();

    // Controls the deterministic random sequence.
    public int Seed { get; }

    public World World { get; }

    public IReadOnlyList<SimulationEvent> Events
        => events;

    // The World owns the actual agents.
    // Expose a stable snapshot for inspection/equivalence.
    public IReadOnlyList<Agent> Agents
        => World.AllAgents().ToArray();

    public Simulation(
        World world,
        int seed)
    {
        World = world;
        Seed = seed;
        random = new Random(seed);
    }

    public Simulation(World world)
    {
        World = world;
        Seed = Random.Shared.Next();
        random = new Random(Seed);
    }

    public void Step()
    {
        var agents = World.AllAgents().ToArray();

        var observations =
            new Dictionary<Agent, Observation>();

        var decisions =
            new Dictionary<Agent, Decision>();

        // --------------------------------------------------
        // Observe
        // --------------------------------------------------

        foreach (var agent in agents)
        {
            var observation = agent.Observe(World);

            observations[agent] = observation;

            Record(
                agent,
                "Observe",
                observation.ToString());
        }

        // --------------------------------------------------
        // Decide
        // --------------------------------------------------

        foreach (var agent in agents)
        {
            var decision =
                agent.Decide(observations[agent]);

            decisions[agent] = decision;

            Record(
                agent,
                "Decision",
                decision.ToString());
        }

        // --------------------------------------------------
        // Act
        // --------------------------------------------------

        foreach (var agent in agents)
        {
            var before = agent.Position;

            agent.Act(decisions[agent]);

            var after = agent.Position;

            Record(
                agent,
                "Action",
                $"{before} -> {after}");
        }

        World.Advance();
    }

    public SimulationResult Run(
        int maxTicks = 1_000)
    {
        while (
            World.Tick < maxTicks &&
            !World.AllAgentsComplete)
        {
            Step();
        }

        return new SimulationResult(
            RunId,
            Seed,
            World.Tick,
            World.AllAgentsComplete,
            World.AllAgents().ToArray()
        );
    }

    private void Record(
        Agent agent,
        string eventType,
        string description)
    {
        events.Add(
            new SimulationEvent(
                RunId,
                Seed,
                World.Tick,
                agent.Id,
                agent.Name,
                eventType,
                description));
    }

    /// <summary>
    /// Compares simulation state.
    /// RunId is intentionally ignored because it identifies an execution,
    /// not the resulting simulation state.
    /// </summary>
    public bool IsEquivalentTo(Simulation other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return
            Seed == other.Seed &&
            World.Tick == other.World.Tick &&
            World.AllAgentsComplete == other.World.AllAgentsComplete &&
            AgentsEquivalentTo(other);
    }

    /// <summary>
    /// Compares the event streams while ignoring RunId.
    /// </summary>
    public bool HasEquivalentEventStream(Simulation other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return Events
            .Select(NormalizeEvent)
            .SequenceEqual(
                other.Events.Select(NormalizeEvent),
                StringComparer.Ordinal);
    }

    /// <summary>
    /// Same seed + same scenario should produce the same state and events.
    /// </summary>
    public bool IsDeterministicWith(Simulation other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return
            IsEquivalentTo(other) &&
            HasEquivalentEventStream(other);
    }

    private bool AgentsEquivalentTo(Simulation other)
    {
        if (Agents.Count != other.Agents.Count)
            return false;

        return Agents
            .Zip(other.Agents)
            .All(pair =>
                AgentEquivalentTo(
                    pair.First,
                    pair.Second));
    }

    private static bool AgentEquivalentTo(
        Agent a,
        Agent b)
    {
        // Agent.Id is deliberately ignored.
        // IDs identify agent instances; Name + Position represent
        // the simulation state we currently care about.
        return
            a.Name == b.Name &&
            a.Position == b.Position;
    }

    private static string NormalizeEvent(
        SimulationEvent @event)
    {
        // RunId identifies the execution and therefore must not affect
        // deterministic event-stream equivalence.
        return @event
            .ToString()
            .Replace(
                @event.RunId.ToString(),
                "<RUN_ID>",
                StringComparison.Ordinal);
    }
}