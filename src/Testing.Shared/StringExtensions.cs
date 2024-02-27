namespace Testing.Shared;

public static class StringExtensions
{
    public static string Squeeze(
        this string source)
    {
        return source.Replace(" ", "");
    }
}