using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Application.Utils;

public partial class TextNormalizer : ITextNormalizer
{
    public string NormalizeText(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        string normalized = input.ToLowerInvariant();
        
        normalized = RemoveDiacritics(normalized);
        
        normalized = LettersAndNumbers().Replace(normalized, "");
        
        normalized = WhiteSpaces().Replace(normalized, " ");
        
        normalized = normalized.Trim().Replace(" ", "-");
        
        return normalized;
    }

    public string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex(@"[^a-z0-9\s]")]
    private static partial Regex LettersAndNumbers();
    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaces();
}