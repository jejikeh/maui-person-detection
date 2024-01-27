using System.Text.RegularExpressions;

namespace PersonDetection.Client.Infrastructure.Common.Extensions;

public static class StringExtensions
{
    public static string RemoveSpecialCharacters(this string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
}