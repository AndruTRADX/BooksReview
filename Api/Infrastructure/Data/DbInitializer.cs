using System;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbInitializer
{

    public static async Task SeedData(ApplicationDbContext appContext, UserManager<User> userManager, RoleManager<IdentityRole<string>> roleManager)
    {
        if (await appContext.Books.AnyAsync()) return;

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
            new Genre { Name = "Science Fiction", Id = "sci-fi" },
            new Genre { Name = "Fantasy", Id = "fantasy" },
            new Genre { Name = "Mystery", Id = "mystery" },
            new Genre { Name = "Thriller", Id = "thriller" },
            new Genre { Name = "Biography", Id = "biography" },
            new Genre { Name = "Non-Fiction", Id = "non-fiction" },
            new Genre { Name = "Romance", Id = "romance" },
            new Genre { Name = "Horror", Id = "horror" }
        };
        await appContext.Genres.AddRangeAsync(genres);

        var books = new[]
        {
            new Book
            {
                Id = "dune_frank-herbert",
                Title = "Dune",
                Author = "Frank Herbert",
                ISBN = "9780441172719",
                PublicationYear = 1965,
                Description = "A science fiction masterpiece",
                BookGenres =
                [
                    new() { Genre = genres[0] }
                ]
            },
            new Book
            {
                Id = "hobbit_jrr-tolkien",
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
        await appContext.Books.AddRangeAsync(books);

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
        await appContext.Reviews.AddRangeAsync(reviews);

        await appContext.SaveChangesAsync();
    }
}