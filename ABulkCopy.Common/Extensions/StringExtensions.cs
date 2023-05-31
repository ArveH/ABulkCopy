namespace ABulkCopy.Common.Extensions;

public static class StringExtensions
{
    public static string Plural(this string str, int cnt)
    {
        return str + (cnt > 1 ? str.EndsWith("x") ? "es" : "s" : "");
    }

    public static bool IsRaw(this string str)
    {
        return str is MssTypes.Binary or MssTypes.VarBinary;
    }
}