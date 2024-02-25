namespace ABulkCopy.Common.Config;

public class ConfigHelper : IConfigHelper
{

    public IConfigurationRoot GetConfiguration(
        string? userSecretsKey = null,
        Dictionary<string, string?>? appSettings = null)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true);

        if (!string.IsNullOrWhiteSpace(userSecretsKey))
        {
            configuration.AddUserSecrets(userSecretsKey);
        }

        configuration.AddEnvironmentVariables();

        if (appSettings != null)
        {
            configuration.AddInMemoryCollection(appSettings);
        }
            
        return configuration.Build();
    }

}