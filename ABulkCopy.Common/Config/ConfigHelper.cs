namespace ABulkCopy.Common.Config;

public class ConfigHelper : IConfigHelper
{

    public IConfigurationRoot GetConfiguration(
        string? userSecretsKey = null,
        string? connectionString = null)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true);

        if (!string.IsNullOrWhiteSpace(userSecretsKey))
        {
            configuration.AddUserSecrets(userSecretsKey);
        }

        configuration.AddEnvironmentVariables();

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BulkCopy"] = connectionString
            });
        }
            
        return configuration.Build();
    }

}