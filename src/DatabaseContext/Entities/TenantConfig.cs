using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseContext.Entities;

public class TenantConfig
{
    [Key]
    [MaxLength(100)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string DisplayName { get; set; }

    [Required]
    [MaxLength(200)]
    public required string ErpDatabaseName { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; private set; }

    [Required]
    public TenantConfigStatus Status { get; private set; }

    public virtual ICollection<TenantSetting> TenantSettings { get; set; } = [];
}

public enum TenantConfigStatus : byte
{
    Provisioning = 0,
    Active = 1,
    ProvisioningFailed = 2,
    Inactive = 3
}
