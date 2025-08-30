using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<QuickPay_API>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
