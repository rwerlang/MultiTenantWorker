using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseContext.Entities;

public class Settings
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required virtual string Uid { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public string? Path { get; set; }

}
