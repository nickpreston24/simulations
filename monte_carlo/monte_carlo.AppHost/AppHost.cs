var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.monte_carlo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

// var python = builder
//     .AddDockerfile("intelisim", "../intelisim")
//     .WithBuildArg("FROM_SCRIPT", "aspire");

// var fournations_sim = builder.AddProject<Projects.FourNations_Simulation>("fournations-simulation");
var fournations_tests = builder.AddProject<Projects.FourNations_Simulation_Tests>("fournations-tests");


var python = builder
    .AddDockerfile(
        "intelisim",
        "../intelisim",
        "flask/Dockerfile")
    .WithBuildArg("FROM_SCRIPT", "1")
    .WithHttpEndpoint(5000, 5000, "http");


// builder.AddProject<Projects.monte_carlo_Web>("webfrontend")
//     .WithExternalHttpEndpoints()
//     .WithHttpHealthCheck("/health")
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();