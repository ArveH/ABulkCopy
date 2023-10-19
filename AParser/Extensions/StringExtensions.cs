using System.Text.RegularExpressions;

namespace AParser.Extensions;

public static class StringExtensions
{
    private static readonly Regex LongDateRegex = new("([0-9]{8}.[0-9]{2}:[0-9]{2}:[0-9]{2}:[0-9]{3})");

    public static string? ExtractLongDateString(this string str)
    {
        var match = LongDateRegex.Match(str);
        return match.Success ? match.Groups[1].Value : null;
    }
}