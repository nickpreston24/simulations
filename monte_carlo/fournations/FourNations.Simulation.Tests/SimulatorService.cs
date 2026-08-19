using CodeMechanic.Async;
using CodeMechanic.Diagnostics;
using CodeMechanic.Shargs;
using FourNations.Simulation;
using Serilog.Core;

public sealed class SimulatorService : QueuedService
{
    private readonly Logger logger;
    private readonly ArgsMap arguments;

    public SimulatorService(ArgsMap arguments, Logger logger)
    {
        this.arguments = arguments;
        this.logger = logger;
        // steps.Add(RunSim1);
        // steps.Add(Compare2Sims);
        steps.Add(RunMonteCarlo);
    }

    private Task RunMonteCarlo()
    {
        const int runs = 10_000;

        var results =
            Enumerable
                .Range(0, runs)
                .Select(seed =>
                {
                    var simulation =
                        new Simulation(
                            FourNationsScenario.Create(),
                            seed);

                    return simulation.Run();
                })
                .ToArray();

        var successful =
            results.Count(x => x.Success);

        var successRate =
            successful / (double)results.Length;

        logger.Information(
            "Monte Carlo: Runs={Runs} Successful={Successful} " +
            "SuccessRate={SuccessRate:P2}",
            runs,
            successful,
            successRate);

        return Task.CompletedTask;
    }

    private Task Compare2Sims()
    {
        var simulation1 =
            new Simulation(
                FourNationsScenario.Create(),
                seed: 12345);

        var result1 =
            simulation1.Run();

        var simulation2 =
            new Simulation(
                FourNationsScenario.Create(),
                seed: 12345);

        var result2 =
            simulation2.Run();

        // Are they identical?
        var identical =
            simulation1.Events
                .Select(x => x.ToString())
                .SequenceEqual(
                    simulation2.Events.Select(x => x.ToString()));

        simulation1.Dump(nameof(simulation1), printFn: logger.Information);

        logger.Information($"{nameof(simulation2)} identical to {nameof(simulation1)} ? :>> {identical}");

        // should be `true'

        var simulation3 =
            new Simulation(
                FourNationsScenario.Create(),
                seed: 54321);
        simulation3.Dump(nameof(simulation3), printFn: logger.Information);

        identical =
            simulation1.Events
                .Select(x => x.ToString())
                .SequenceEqual(
                    simulation3.Events.Select(x => x.ToString()));

        logger.Information($"{nameof(simulation3)} identical to {nameof(simulation1)} ? :>> {identical}");


        return Task.CompletedTask;
    }

    private Task Run100Sims()
    {
        var results =
            Enumerable
                .Range(0, 100)
                .Select(seed =>
                {
                    var simulation =
                        new Simulation(
                            FourNationsScenario.Create(),
                            seed);

                    var result =
                        simulation.Run();

                    return result;
                })
                .ToArray();


        var successful =
            results.Count(x => x.Success);

        var successRate =
            successful / (double)results.Length;

        logger.Information(
            $"Successful runs: {successful}/{results.Length}");

        logger.Information(
            $"Success rate: {successRate:P2}");

        return Task.CompletedTask;
    }

    private async Task RunSim1()
    {
        var simulation =
            new Simulation(
                FourNationsScenario.Create());

        var result =
            simulation.Run();

        foreach (var @event in simulation.Events)
        {
            logger.Information(@event.ToString());
        }

        logger.Information(result.ToString());

        await Task.CompletedTask; // stand-in to make async keyword happy.  non-functional.
    }
}