using BookStore.PostgreSql;
using BookStore.PostgreSql.MigrationService;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var authSection = configuration.GetSection(nameof(AuthorizationOptions));
if (!authSection.Exists())
{
    throw new InvalidOperationException("AuthorizationOptions section is missing in configuration.");
}

builder.Services.Configure<AuthorizationOptions>(
    authSection);
var a = authSection.Get<AuthorizationOptions>();


builder.AddNpgsqlDbContext<BookStoreDbContext>("BookStoreDb", options =>
{
    options.DisableTracing = true;
    
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();