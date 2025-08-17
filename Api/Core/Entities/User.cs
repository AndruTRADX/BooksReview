using System;
using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = null!;
    public DateTime MemberSince { get; set; } = DateTime.UtcNow;
    public UserStatus Status { get; set; } = UserStatus.Active;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; } = new List<IdentityUserRole<string>>();
}
