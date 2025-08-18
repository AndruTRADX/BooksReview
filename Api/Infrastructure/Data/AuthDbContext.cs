// Infrastructure/Data/AuthDbContext.cs
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) 
    : IdentityDbContext<User, IdentityRole<string>, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        var roles = new List<IdentityRole<string>>()
        {
            new() {
                Id = "admin",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new() {
                Id = "user",
                Name = "User",
                NormalizedName = "USER"
            },
            new() {
                Id = "moderator",
                Name = "Moderator",
                NormalizedName = "MODERATOR"
            }
        };

        builder.Entity<IdentityRole<string>>().HasData(roles);
        
        builder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();
    }
}