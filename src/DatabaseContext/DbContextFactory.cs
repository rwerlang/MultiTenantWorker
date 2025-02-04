using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseContext;

public class DbContextFactory (
    IServiceProvider serviceProvider,
    IConfiguration configuration
)
{
    public IServiceScope CreateScope()
    {
        return serviceProvider.CreateScope();
    }

    public ConfigDbContext CreateConfigDbContext(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ConfigDbContext>();
        return dbContext;
    }

    public async Task<TenantDbContext> CreateTenantDbContextAsync(IServiceProvider serviceProvider, string tenantId)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentNullException(nameof(tenantId));
        
        var configContext = CreateConfigDbContext(serviceProvider);
        var tenant = await configContext.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tenantId) 
            ?? throw new InvalidOperationException("tenant_not_found");

        var connStr = configuration.GetConnectionString("Tenant")!;
        var tenantDbName = tenant.ErpDatabaseName;
        var dbContext = serviceProvider.GetRequiredService<TenantDbContext>();

        connStr = connStr.Replace("[#database#]", tenantDbName);

        dbContext.Database.SetConnectionString(connStr);
        return dbContext;   
    }
}
