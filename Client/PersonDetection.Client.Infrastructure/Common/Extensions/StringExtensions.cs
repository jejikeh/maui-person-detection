using System.Text.RegularExpressions;

namespace PersonDetection.Client.Infrastructure.Common.Extensions;

public static partial class StringExtensions
{
    public static string RemoveSpecialCharacters(this string str)
    {
        return LettersAndNumbersRegex().Replace(str, "");
    }

    [GeneratedRegex("[^a-zA-Z0-9_.]+", RegexOptions.Compiled)]
    private static partial Regex LettersAndNumbersRegex();
}