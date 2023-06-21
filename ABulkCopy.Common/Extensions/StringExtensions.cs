namespace ABulkCopy.Common.Extensions;

public static class StringExtensions
{
    private static readonly Regex SingleQuoteRegex = new("('[^']*')");

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

    public static string TrimParentheses(this string str)
    {
        var offset = str.TakeWhile(c => c == '(').Count();
        return str[offset..^offset];
    }

    public static string? ExtractDateString(this string str)
    {
        var match = SingleQuoteRegex.Match(str);
        return match.Success ? match.Groups[1].Value : null;
    }
}