using System;
using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext;

public class ConfigDbContext : DbContext
{
    public DbSet<TenantConfig> Tenants { get; set; }
    public DbSet<TenantSetting> TenantsSettings { get; set; }


    
    public ConfigDbContext(DbContextOptions<ConfigDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantConfig>().ToTable("Tenant");
        modelBuilder.Entity<TenantSetting>().ToTable("TenantSettings");
        modelBuilder.Entity<TenantSetting>().HasKey(ts => new { ts.TenantId, ts.Key });
        
        modelBuilder.Entity<TenantSetting>()
            .HasOne(ts => ts.Tenant)
            .WithMany(t => t.TenantSettings)
            .HasForeignKey(ts => ts.TenantId);

        base.OnModelCreating(modelBuilder);
    }

}
