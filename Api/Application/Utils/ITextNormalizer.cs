using System;

namespace Application.Utils;

public interface ITextNormalizer
{
    string NormalizeText(string input);
    string RemoveDiacritics(string text);
}
