using Serilog.Core;

public class Application
{
    private readonly Logger logger;

    private readonly SimulatorService simulations;

    public Application(Logger logger
        , SimulatorService simulations
    )
    {
        this.logger = logger;
        this.simulations = simulations;
    }

    public async Task Run()
    {
        logger.Information($"{nameof(Application)} running... ");
        await simulations.Run();
        logger.Information("All services done running.");
    }
}