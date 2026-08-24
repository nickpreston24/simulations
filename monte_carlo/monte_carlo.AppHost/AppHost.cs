using CodeMechanic.FileSystem;
using CodeMechanic.Shargs;

var argsmap = new ArgsMap(args);
bool debug = argsmap.HasFlag("--debug");
DotEnv.Load(debug: debug);

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.monte_carlo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var fournations_tests = builder.AddProject<Projects.FourNations_Simulation_Tests>("fournations-tests");

var intelisim = builder
    .AddDockerfile(
        "intelisim",
        "../intelisim",
        "Dockerfile")
    .WithBuildArg("FROM_SCRIPT", "1")
    .WithHttpEndpoint(5000, 5000, "http");

// var palworld_pb = builder
//     .AddContainer("pocketbase", "ghcr.io/muchobien/pocketbase")
//     .WithBindMount("./data/pocketbase", "/pb_data")
//     .WithHttpEndpoint(targetPort: 8090, name: "http");

var palworld_pb = builder
    .AddContainer("pocketbase", "ghcr.io/muchobien/pocketbase")
    .WithBindMount("./data/pocketbase", "/pb_data")
    .WithEnvironment("PB_ADMIN_EMAIL", "admin@localhost")
    .WithEnvironment("PB_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(targetPort: 8090, name: "http");

var palworld_s3_bucket = builder
    .AddContainer("s3", "minio/minio")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithBindMount("./data/s3", "/data")
    .WithHttpEndpoint(targetPort: 9000, name: "api")
    .WithHttpEndpoint(targetPort: 9001, name: "console");

builder.AddProject<Projects.PalCentral>("palcentral")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithEnvironment("POCKETBASE_URL", palworld_pb.GetEndpoint("http"))
    .WithEnvironment("S3_ENDPOINT", palworld_s3_bucket.GetEndpoint("api"))
    .WaitFor(apiService);

// builder.AddProject<Projects.PalCentral>("palcentral")
//     .WithExternalHttpEndpoints()
//     .WithHttpHealthCheck("/health")
//     .WithReference(palworld_pb)
//     .WaitFor(apiService); // dummy api

builder.Build().Run();