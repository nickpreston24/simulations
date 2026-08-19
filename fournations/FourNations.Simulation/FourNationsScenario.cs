namespace FourNations.Simulation;

public static class FourNationsScenario
{
    public static World Create()
    {
        var world = new World();

        var north = new Nation("North");
        var south = new Nation("South");
        var east = new Nation("East");
        var west = new Nation("West");

        world.Nations.AddRange([
            north,
            south,
            east,
            west
        ]);

        north.Agents.Add(
            new Agent(
                "North-1",
                north,
                new Position(0, 0),
                new Position(10, 10)));

        south.Agents.Add(
            new Agent(
                "South-1",
                south,
                new Position(0, 10),
                new Position(10, 0)));

        east.Agents.Add(
            new Agent(
                "East-1",
                east,
                new Position(10, 0),
                new Position(0, 10)));

        west.Agents.Add(
            new Agent(
                "West-1",
                west,
                new Position(10, 10),
                new Position(0, 0)));

        return world;
    }
}