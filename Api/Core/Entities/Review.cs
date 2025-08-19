using System;
using Core.Common;
using Core.Enums;

namespace Core.Entities;

public class Review : BaseAuditableEntity
{
    public bool IsRecommended { get; set; }
    public string Content { get; set; } = null!;
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    
    public string BookId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<ReviewReport> Reports { get; set; } = [];
}
