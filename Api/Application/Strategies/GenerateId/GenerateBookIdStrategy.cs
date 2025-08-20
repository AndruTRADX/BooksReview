using System;
using Application.Utils;
using Core.Entities;

namespace Application.Strategies.GenerateId;

public class GenerateBookIdStrategy(ITextNormalizer textNormalizer) : IGenerateIdStrategy<Book>
{
    public string GenerateId(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Title cannot be null or empty", nameof(book.Title));
        
        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Book author cannot be null or empty", nameof(book.Author));

        string normalizedTitle = textNormalizer.NormalizeText(book.Title);
        
        string normalizedAuthor = textNormalizer.NormalizeText(book.Author);

        string baseId = $"{normalizedTitle}_{normalizedAuthor}";

        if (!string.IsNullOrWhiteSpace(book.ISBN))
        {
            return $"{baseId}_{book.ISBN}";
        }
        
        return baseId;
    }
}