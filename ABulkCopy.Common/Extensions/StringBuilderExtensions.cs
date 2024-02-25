namespace ABulkCopy.Common.Extensions;

public static class StringBuilderExtensions
{
    public static bool IsNullOrWhitespace(this StringBuilder sb)
    {
        for (var i = 0; i < sb.Length; i++)
        {
            if (!char.IsWhiteSpace(sb[i])) return false;
        }

        return true;
    }
}