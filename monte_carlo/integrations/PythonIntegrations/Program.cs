using CodeMechanic.FileSystem;
using Python.Runtime;
using Serilog;


DotEnv.Load(debug: true);
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "./logs/python-integrations.log",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true
    )
    .CreateLogger();

Console.WriteLine("Hello, from PythonIntegrations!");

string dll_version = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL") ?? "";
logger.Information($"{nameof(dll_version)} :>> {dll_version}");

PythonEngine.Initialize();
using (Py.GIL())
{
    dynamic np = Py.Import("numpy");
    Console.WriteLine(np.cos(np.pi * 2));

    dynamic sin = np.sin;
    Console.WriteLine(sin(5));

    double c = (double)(np.cos(5) + sin(5));
    Console.WriteLine(c);

    dynamic a = np.array(new List<float> { 1, 2, 3 });
    Console.WriteLine(a.dtype);

    dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
    Console.WriteLine(b.dtype);

    Console.WriteLine(a * b);
    Console.ReadKey();
}

Console.WriteLine("Numpy test complete!");