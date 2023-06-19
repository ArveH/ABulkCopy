using System.Numerics;

namespace ABulkCopy.Common.Extensions;

public static class StringExtensions
{
    public static string Plural<T>(this string str, T cnt)
        where T : INumber<T>
    {
        return str + (cnt != T.CreateChecked(1) ? str.EndsWith("x") ? "es" : "s" : "");
    }

    public static bool IsRaw(this string str)
    {
        return str is MssTypes.Binary or MssTypes.VarBinary or PgTypes.ByteA;
    }

    public static string Quote(this string str)
    {
        return Constants.Quote + str + Constants.Quote;
    }
}