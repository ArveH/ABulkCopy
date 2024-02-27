namespace ABulkCopy.Common.Extensions;

public static class DictionaryExtensions
{
    public static string? NullableGet(this Dictionary<string, string?> dictionary, string? key)
    {
        if (key == null) return null;

        return dictionary.TryGetValue(key, out var value) ? value : null;
    }

    public static string GetKeyIfValueNotExist(this Dictionary<string, string> dictionary, string key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : key;
    }

    public static string? NullableGetKeyIfValueNotExist(this Dictionary<string, string?> dictionary, string? key)
    {
        if (key == null) return null;
        return dictionary.TryGetValue(key, out var value) ? value : key;
    }
}