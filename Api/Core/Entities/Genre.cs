using System;
using Core.Common;

namespace Core.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
