using System;
using Core.Common;

namespace Core.Entities;

public class Book : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? ISBN { get; set; }
    public int PublicationYear { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
