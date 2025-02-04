using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext;

public class TenantDbContext : DbContext
{
    public DbSet<Settings> Settings { get; set; }


    public TenantDbContext(DbContextOptions options) : base(options) {}
    public TenantDbContext() : base() {}
}
