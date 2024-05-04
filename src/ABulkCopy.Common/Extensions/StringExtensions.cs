using System.Collections.Generic;

namespace ABulkCopy.Common.Extensions;

public static class StringExtensions
{
    private static readonly Regex SingleQuoteRegex = new("('[^']*')");
    private static readonly Regex LongDateRegex = new("('[0-9]{8}.[0-9]{2}:[0-9]{2}:[0-9]{2}:[0-9]{3}[^']*')");

    public static string Plural<T>(this string str, T cnt)
        where T : INumber<T>
    {
        return str + (cnt != T.CreateChecked(1) ? str.EndsWith("x") ? "es" : "s" : "");
    }

    public static bool IsRaw(this string str)
    {
        return str is MssTypes.Binary or MssTypes.VarBinary or MssTypes.Image or PgTypes.ByteA;
    }

    public static string Quote(this string str)
    {
        return Constants.Quote + str + Constants.Quote;
    }

    public static string AddSchemaFilter(this string? str)
    {
        return string.IsNullOrWhiteSpace(str) 
            ? "AND s.name not in ('guest', 'INFORMATION_SCHEMA', 'sys', 'logs') " // guest, information_schema, sys, logs
            : "AND s.name in (" + string.Join(",", str.Split(',').Select(s => $"'{s}'")) + ") ";
    }

    public static string TrimSchema(this string str)
    {
        var parts = str.Split('.');
        return parts.Length == 2 ? parts[1] : str;
    }

    public static string TrimParentheses(this string str)
    {
        var offset = str.TakeWhile(c => c == '(').Count();
        return str[offset..^offset];
    }

    public static string? ExtractSingleQuoteString(this string str)
    {
        var match = SingleQuoteRegex.Match(str);
        return match.Success ? match.Groups[1].Value : null;

    }
    public static string? ExtractLongDateString(this string str)
    {
        var match = LongDateRegex.Match(str);
        return match.Success ? match.Groups[1].Value : null;
    }
}