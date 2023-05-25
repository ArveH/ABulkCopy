namespace ABulkCopy.Common.Extensions;

public static class StringExtensions
{
    public static string Plural(this string str, int cnt)
    {
        return str + (cnt > 1 ? "s" : "");
    }
}