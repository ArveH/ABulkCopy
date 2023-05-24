using ABulkCopy.Cmd.Config;

namespace ABulkCopy.Cmd;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets("128e015d-d8ef-4ca8-ba79-5390b26c675f")
            .AddEnvironmentVariables()
            .Build();

        var parser = new Parser(cfg =>
        {
            cfg.CaseInsensitiveEnumValues = true;
            cfg.HelpWriter = Console.Error;
        });
        var result = parser.ParseArguments<CmdOptions>(args);
        if (result.Tag == ParserResultType.NotParsed)
        {
            // A usage message is written to Console.Error by the CommandLineParser
            return;
        }

        var cmdParameters = result.Value;

        ConfigureLogger(configuration, cmdParameters.LogFile);

        try
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddConfiguration(configuration);
            builder.Logging.AddSerilog();

            ConfigureServices(builder, configuration);

            var host = builder.Build();
            var logger = host.Services.GetRequiredService<ILogger>();

            logger.Information("ABulkCopy.Cmd started");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void ConfigureServices(HostApplicationBuilder builder, IConfigurationRoot configuration)
    {
        builder.Services.AddSingleton(configuration);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddSingleton<IFileSystem>(new FileSystem());
        builder.Services.AddSingleton<ISchemaWriter, SchemaWriter>();
        builder.Services.AddSingleton<IDataWriter, DataWriter>();
        builder.Services.AddSingleton<ISelectCreator, SelectCreator>();
        builder.Services.AddMssServices();
    }

    private static void ConfigureLogger(
        IConfigurationRoot configuration,
        string fileFullPath)
    {
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File(fileFullPath,
                outputTemplate: outputTemplate,
                fileSizeLimitBytes: 10000000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1));
        Log.Logger = loggerConfig.CreateLogger();    }
}