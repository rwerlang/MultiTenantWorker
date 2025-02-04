using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseContext.Entities;

public class TenantSetting
{
    [Key]
    [MaxLength(100)]
    public required string TenantId { get; set; }

    [Key]
    [MaxLength(50)]
    public required string Key { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Value { get; set; }

    public TenantConfig? Tenant { get; set; }

}
