namespace ABulkCopy.Common.Config;

public interface IConfigHelper
{
    IConfigurationRoot GetConfiguration(
        string? userSecretsKey = null, string? connectionString = null);
}