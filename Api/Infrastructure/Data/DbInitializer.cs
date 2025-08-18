using System;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedData(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await context.Books.AnyAsync()) return;

        var roles = new[] { "Admin", "User", "Moderator" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminUser = new User
        {
            UserName = "AndruTRADX",
            Email = "andrudeluxe@gmail.com",
            DisplayName = "Administrator",
            MemberSince = DateTime.UtcNow,
            Status = UserStatus.Active
        };

        await userManager.CreateAsync(adminUser, "BookReview123!");
        await userManager.AddToRolesAsync(adminUser, roles);

        var genres = new[]
        {
            new Genre { Name = "Science Fiction" },
            new Genre { Name = "Fantasy" },
            new Genre { Name = "Mystery" },
            new Genre { Name = "Thriller" },
            new Genre { Name = "Biography" }
        };
        await context.Genres.AddRangeAsync(genres);

        var books = new[]
        {
            new Book
            {
                Title = "Dune",
                Author = "Frank Herbert",
                ISBN = "9780441172719",
                PublicationYear = 1965,
                Description = "A science fiction masterpiece",
                BookGenres = new List<BookGenre>
                {
                    new() { Genre = genres[0] }
                }
            },
            new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                ISBN = "9780547928227",
                PublicationYear = 1937,
                Description = "Fantasy adventure novel",
                BookGenres =
                [
                    new() { Genre = genres[1] }
                ]
            }
        };
        await context.Books.AddRangeAsync(books);

        var reviews = new[]
        {
            new Review
            {
                Book = books[0],
                User = adminUser,
                Content = "One of the greatest sci-fi novels ever written",
                IsRecommended = true,
                Status = ReviewStatus.Approved
            },
            new Review
            {
                Book = books[1],
                User = adminUser,
                Content = "A perfect introduction to fantasy literature",
                IsRecommended = true,
                Status = ReviewStatus.Approved
            }
        };
        await context.Reviews.AddRangeAsync(reviews);

        await context.SaveChangesAsync();
    }
}