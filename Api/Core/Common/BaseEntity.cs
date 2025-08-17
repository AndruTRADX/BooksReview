using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Common;

public class BaseEntity
{
    [Key]
    public string Id { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
