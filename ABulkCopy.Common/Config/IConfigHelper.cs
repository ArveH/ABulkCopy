namespace ABulkCopy.Common.Config;

public interface IConfigHelper
{
    IConfigurationRoot GetConfiguration(
        string? userSecretsKey = null,
        Dictionary<string, string?>? appSettings = null);
}