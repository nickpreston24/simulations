namespace FourNations.Simulation;

public sealed class Agent
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; }

    public Nation Nation { get; }

    public Position Position { get; private set; }

    public Position Target { get; }

    public bool HasReachedTarget
        => Position == Target;

    public Agent(
        string name,
        Nation nation,
        Position position,
        Position target)
    {
        Name = name;
        Nation = nation;
        Position = position;
        Target = target;
    }

    public Observation Observe(World world)
    {
        return new Observation(
            world.Tick,
            Position,
            Target);
    }

    public Decision Decide(Observation observation)
    {
        var position = observation.Position;
        var target = observation.Target;

        if (position == target)
            return new Decision(
                new AgentAction.Stay());

        if (position.X < target.X)
        {
            return new Decision(
                new AgentAction.MoveTo(
                    position with
                    {
                        X = position.X + 1
                    }));
        }

        if (position.X > target.X)
        {
            return new Decision(
                new AgentAction.MoveTo(
                    position with
                    {
                        X = position.X - 1
                    }));
        }

        if (position.Y < target.Y)
        {
            return new Decision(
                new AgentAction.MoveTo(
                    position with
                    {
                        Y = position.Y + 1
                    }));
        }

        return new Decision(
            new AgentAction.MoveTo(
                position with
                {
                    Y = position.Y - 1
                }));
    }

    public void Act(Decision decision)
    {
        switch (decision.Action)
        {
            case AgentAction.Stay:
                break;

            case AgentAction.MoveTo move:
                Position = move.Destination;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string ToString()
        => $"{Nation.Name}/{Name} " +
           $"Position={Position} " +
           $"Target={Target}";
}

//
// public sealed class Agent
// {
//     public Guid Id { get; } = Guid.NewGuid();
//
//     public string Name { get; }
//
//     public Nation Nation { get; }
//
//     public Position Position { get; private set; }
//
//     public Agent(
//         string name,
//         Nation nation,
//         Position position)
//     {
//         Name = name;
//         Nation = nation;
//         Position = position;
//     }
//
//     public Observation Observe(World world)
//     {
//         return new Observation(
//             Tick: world.Tick,
//             Position: Position);
//     }
//
//     public Decision Decide(Observation observation)
//     {
//         // Dumb Version-1 behavior:
//         // always move east.
//         // return new Decision(
//         //     AgentAction.MoveEast);
//         return default;
//     }
//
//     public void Act(Decision decision, World world)
//     {
//         switch (decision.Action)
//         {
//             case AgentAction.MoveEast:
//                 Position = Position with
//                 {
//                     X = Position.X + 1
//                 };
//                 break;
//         }
//     }
// }