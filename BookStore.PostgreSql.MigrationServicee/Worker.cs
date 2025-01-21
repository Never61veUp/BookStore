using System.Diagnostics;
using BookStore.Core.Enums;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace BookStore.PostgreSql.MigrationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private AuthorizationOptions _authOptions;

    public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider,
        IOptions<AuthorizationOptions> authOptions)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _serviceProvider = serviceProvider;
        if (authOptions.Value.RolePermissions.Count() == 0)
        {
            _logger.LogError("RolePermissions collection is empty!");
        }
        else
        {
            _logger.LogInformation("RolePermissions loaded successfully.");
        }

        _authOptions = authOptions.Value;
    }

    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(BookStoreDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(BookStoreDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private async Task SeedDataAsync(BookStoreDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!await dbContext.Roles.AnyAsync(cancellationToken))
        {
            var roles = Enum
                .GetValues<Role>()
                .Select(r => new RoleEntity((int)r)
                {
                    Name = r.ToString()
                });
            await dbContext.Roles.AddRangeAsync(roles, cancellationToken);
        }
        if (!await dbContext.Permissions.AnyAsync(cancellationToken))
        {
            var permissions = Enum
                .GetValues<Permission>()
                .Select(p => new PermissionEntity((int)p)
                {
                    Name = p.ToString()
                });
            await dbContext.Permissions.AddRangeAsync(permissions, cancellationToken);
        }
        if (!await dbContext.RolePermissions.AnyAsync(cancellationToken))
        {
            var parseRolePermissions = ParseRolePermissions();
            await dbContext.RolePermissions.AddRangeAsync(parseRolePermissions, cancellationToken);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    private RolePermissionEntity[] ParseRolePermissions()
    {
        return _authOptions.RolePermissions
            .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p)
                }))
            .ToArray();
    }
}