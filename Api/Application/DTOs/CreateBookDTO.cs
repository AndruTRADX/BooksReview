using System;

namespace Application.DTOs;

public class CreateBookDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? ISBN { get; set; }
    public int PublicationYear { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
    public List<string> GenreIds { get; set; } = [];
}