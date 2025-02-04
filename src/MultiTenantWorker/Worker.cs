using DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MultiTenantWorker;

public class Worker (
    ILogger<Worker> logger,
    DbContextFactory dbContextFactory

) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // We need to create a scoped service provider because EF DbContext requires a scope.
        // Background Workers are singletons, not scoped services,
        // so if we try to use a DbContext without a scoped service context, it will fail.
        using var scope = dbContextFactory.CreateScope();

        // Test the connection to the tenant configuration database
        var configDbContext = dbContextFactory.CreateConfigDbContext(scope.ServiceProvider);
        var tenants = await configDbContext.Tenants.ToListAsync();

        logger.LogInformation("Tenants: {tenantCount}", tenants.Count);


        // Scenario 1
        // This simulates a message received from Service Bus.
        // This message can contain its own structure, and must contain the tenantId.
        // We will use the tenantId to get the SQL database name to connect to the expected database.
        var tenantId = "dulacia";


        var tenantDbContext = await dbContextFactory.CreateTenantDbContextAsync(scope.ServiceProvider, tenantId);
        var settings = await tenantDbContext.Settings.ToListAsync();
        
        foreach (var item in settings)
        {
            var str = $"{item.Id}  {item.Name}  {item.Value}  {item.Path}";
            logger.LogInformation(str);
        }


        // Scenario 2
        // Get all tenants using ConfigDbContext
        // For each tenant, create a new TenantDbContext to run the business rules
        // In this case, create a new scope for each item of the loop, to dispose the connection
        // with the previous database as it is not needed any more after it was used.
    }
}
