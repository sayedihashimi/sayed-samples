var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireCliReact01_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AspireCliReact01_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddNpmApp("my-frontend", "../my-frontend/", "dev")
    .WithReference(apiService)
    .WithHttpEndpoint(targetPort: 5173);

builder.Build().Run();
