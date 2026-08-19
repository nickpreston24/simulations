using CodeMechanic.Diagnostics;
using CodeMechanic.Shargs;
using JsonFlatFileDataStore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

internal class Program
{
    static async Task Main(string[] args)
    {
        var arguments = new ArgsMap(args);

        var tool = new ToolSettings(name: "fournations", settingsFilename: "settings.json");
        tool.Dump(nameof(tool));

        var logFolder =
            Path.Combine(tool.dotfolder, "logs");

        Directory.CreateDirectory(logFolder);

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(logFolder, "fournations.log"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true
            )
            .CreateLogger();

        if (!Directory.Exists(tool.dotfolder))
        {
            logger.Warning($"Dotfolder {tool.dotfolder} does not exist ... creating it...");
            Directory.CreateDirectory(tool.dotfolder);
            logger.Information($"Dotfolder '{tool.dotfolder}' created for tool {tool.name}!");
        }

        var settings_store = new DataStore(tool.tool_settings_path);

        logger.Information($"settings is at {tool.tool_settings_path}");
        logger.Information($"dotfolder is at {tool.dotfolder}");

        await RunAsCli(arguments, logger, tool);
    }

    static async Task RunAsCli(ArgsMap arguments, Logger logger, ToolSettings tool_settings)
    {
        var services = CreateServices(arguments, logger, tool_settings);
        Application app = services.GetRequiredService<Application>();
        await app.Run();
    }


    private static ServiceProvider CreateServices(
        ArgsMap arguments,
        Logger logger,
        ToolSettings tool_settings)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton(arguments)
            .AddSingleton(tool_settings)
            .AddSingleton<Logger>(logger)
            .AddSingleton<SimulatorService>()
            .AddSingleton<Application>()
            .BuildServiceProvider();

        return serviceProvider;
    }
}