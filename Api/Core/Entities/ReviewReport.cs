using System;
using Core.Common;
using Core.Enums;

namespace Core.Entities;

public class ReviewReport: BaseEntity
{
    public string Reason { get; set; } = null!;
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    
    public string ReviewId { get; set; } = null!;
    public string ReportedByUserId { get; set; } = null!;
    
    public Review Review { get; set; } = null!;
    public User ReportedByUser { get; set; } = null!;
}