namespace ABulkCopy.Common.Config;

public static class ConfigurationExtensions
{
    public static string Check(this IConfiguration config, string key)
    {
        var val = config[key];
        if (string.IsNullOrWhiteSpace(val))
        {
            throw new ArgumentException($"Configuration value for '{key}' is missing or empty.");
        }

        return val;
    }

    public static string SafeGet(this IConfiguration config, string key)
    {
        var val = config[key];
        if (string.IsNullOrWhiteSpace(val))
        {
            return "";
        }

        return val;
    }

    public static bool IsTrue(this IConfiguration config, string key)
    {
        var val = config[key];
        if (string.IsNullOrWhiteSpace(val))
        {
            return false;
        }

        return bool.TryParse(val, out var flag) && flag;
    }

    public static EmptyStringFlag ToEnum(this IConfiguration config, string key)
    {
        var val = config[key];
        if (string.IsNullOrWhiteSpace(val) || !Enum.TryParse<EmptyStringFlag>(val, true, out var flag))
        {
            return EmptyStringFlag.Leave;
        }

        return flag;
    }
}