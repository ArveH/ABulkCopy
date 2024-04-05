namespace ABulkCopy.Cmd.Internal.Config;

public class LogConfigHelper
{
    public static ILogger ConfigureLogger(
        IConfigurationRoot configuration,
        string fileFullPath)
    {
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration, 
                new ConfigurationReaderOptions(ConfigurationAssemblySource.AlwaysScanDllFiles))
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File(fileFullPath,
                outputTemplate: outputTemplate,
                fileSizeLimitBytes: 100000000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1));
        return loggerConfig.CreateLogger();
    }
}