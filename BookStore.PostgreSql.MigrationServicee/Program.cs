using BookStore.PostgreSql;
using BookStore.PostgreSql.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<BookStoreDbContext>("BookStoreDb");

var host = builder.Build();
host.Run();