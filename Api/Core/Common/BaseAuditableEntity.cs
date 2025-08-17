using System;

namespace Core.Common;

public class BaseAuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}
