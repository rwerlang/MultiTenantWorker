using DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MultiTenantWorker;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<ConfigDbContext>(options => 
{
    options.UseSqlServer(config.GetConnectionString("TenantConfig"), 
    sql => 
    {
        sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    });
});

builder.Services.AddDbContext<TenantDbContext>(options =>
{
    options.UseSqlServer(sql => 
        sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
});

builder.Services.AddSingleton<DbContextFactory>();

builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();
