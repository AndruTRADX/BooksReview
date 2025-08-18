using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User : IdentityUser<string>
{
    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string DisplayName { get; set; } = null!;
    public DateTime MemberSince { get; set; } = DateTime.UtcNow;
    public UserStatus Status { get; set; } = UserStatus.Active;
}