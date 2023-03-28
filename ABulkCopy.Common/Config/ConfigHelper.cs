namespace ABulkCopy.Common.Config;

public class ConfigHelper : IConfigHelper
{
    public IConfigurationRoot GetConfiguration(string? userSecretsKey = null)
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        if (!string.IsNullOrWhiteSpace(envName))
            builder.AddJsonFile($"appsettings.{envName}.json", optional: true);

        if (!string.IsNullOrWhiteSpace(userSecretsKey))
            builder.AddUserSecrets(userSecretsKey);

        return builder.Build();
    }
}