using ABulkCopy.Cmd.Config;
using ABulkCopy.Cmd.Factories;

namespace ABulkCopy.Cmd;

internal class Program
{
    static async Task Main(string[] args)
    {
        var configuration = GetConfiguration();
        var cmdParameters = GetArguments(args);
        if (cmdParameters == null) return;
        ConfigureLogger(configuration, cmdParameters.LogFile);

        try
        {
            var host = GetHost(configuration);

            if (cmdParameters.Direction == CopyDirection.In)
            {
                Log.Information("ABulkCopy.Cmd started. Copy direction: In");
                throw new NotImplementedException("Copy In not implemented");
            }

            var copyOut = host.Services.GetRequiredService<ICopyOut>();
            await copyOut.Run(cmdParameters.Folder, cmdParameters.SearchStr);
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

    private static IConfigurationRoot GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets("128e015d-d8ef-4ca8-ba79-5390b26c675f")
            .AddEnvironmentVariables()
            .Build();
        return configuration;
    }

    private static CmdArguments? GetArguments(string[] args)
    {
        var parser = new Parser(cfg =>
        {
            cfg.CaseInsensitiveEnumValues = true;
            cfg.HelpWriter = Console.Error;
        });
        var result = parser.ParseArguments<CmdArguments>(args);
        if (result.Tag == ParserResultType.NotParsed)
        {
            // A usage message is written to Console.Error by the CommandLineParser
            return null;
        }

        return result.Value;
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
        Log.Logger = loggerConfig.CreateLogger();
    }

    private static void ConfigureServices(HostApplicationBuilder builder, IConfigurationRoot configuration)
    {
        builder.Services.AddSingleton(configuration);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddSingleton<IFileSystem>(new FileSystem());
        builder.Services.AddSingleton<ISchemaWriter, SchemaWriter>();
        builder.Services.AddSingleton<IDataWriter, DataWriter>();
        builder.Services.AddSingleton<ITableReaderFactory, TableReaderFactory>();
        builder.Services.AddSingleton<ISelectCreator, SelectCreator>();
        builder.Services.AddMssServices();
    }

    private static IHost GetHost(IConfigurationRoot configuration)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddConfiguration(configuration);
        builder.Logging.AddSerilog();

        ConfigureServices(builder, configuration);

        var host = builder.Build();
        return host;
    }
}