namespace ABulkCopy.Common.Extensions;

public static class DictionaryExtensions
{
    public static string? SafeGet(this Dictionary<string, string?> dictionary, string? key)
    {
        if (key == null) return null;

        return dictionary.TryGetValue(key, out var value) ? value : null;
    }
}