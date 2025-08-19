using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.DTOs;

namespace Application.Books.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDTO>
{
    private readonly ApplicationDbContext _context;

    public CreateBookValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MustAsync(BeUniqueTitle).WithMessage("Book with this title already exists");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters");

        RuleFor(x => x.ISBN)
            .Matches(@"^(\d{10}|\d{13})$").When(x => !string.IsNullOrEmpty(x.ISBN))
            .WithMessage("ISBN must be 10 or 13 digits")
            .MustAsync(BeUniqueIsbn).When(x => !string.IsNullOrEmpty(x.ISBN))
            .WithMessage("ISBN already exists");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1000, DateTime.Now.Year)
            .WithMessage($"Publication year must be between 1000 and {DateTime.Now.Year}");

        RuleFor(x => x.CoverImageUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.CoverImageUrl))
            .WithMessage("Invalid URL format");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage("At least one genre is required")
            .MustAsync(AllGenresExist).WithMessage("One or more genres are invalid");
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return !await _context.Books.AnyAsync(b => b.Title == title, cancellationToken);
    }

    private async Task<bool> BeUniqueIsbn(string? isbn, CancellationToken cancellationToken)
    {
        return !await _context.Books.AnyAsync(b => b.ISBN == isbn, cancellationToken);
    }

    private async Task<bool> AllGenresExist(List<string> genreIds, CancellationToken cancellationToken)
    {
        if (genreIds == null || !genreIds.Any()) return false;

        var existingCount = await _context.Genres
            .Where(g => genreIds.Contains(g.Id))
            .CountAsync(cancellationToken);

        return existingCount == genreIds.Count;
    }
}