namespace ABulkCopy.Cmd;

internal class Program
{
    static async Task Main(string[] args)
    {
        var cmdArguments = GetArguments(args);
        if (cmdArguments == null) return;
        var configuration = GetConfiguration(cmdArguments);
        var logger = ConfigureLogger(configuration, cmdArguments.LogFile);
        Log.Logger = logger;
        Log.Information("ABulkCopy.Cmd started.");
        Console.WriteLine("ABulkCopy.Cmd started.");

        try
        {
            var builder = CreateAppBuilder(
                cmdArguments.Rdbms,
                configuration);
            var host = builder.Build();

            if (cmdArguments.Direction == CopyDirection.In)
            {
                var copyIn = host.Services.GetRequiredService<ICopyIn>();
                await copyIn.Run(cmdArguments.Folder, cmdArguments.Rdbms);
            }
            else
            {
                var copyOut = host.Services.GetRequiredService<ICopyOut>();
                if (DataFolder.CreateIfNotExists(cmdArguments.Folder) == CmdStatus.ShouldExit)
                {
                    Log.Information("Folder {Folder} didn't exist. ABulkCopy.Cmd finished.",
                        cmdArguments.Folder);
                    Console.WriteLine($"Folder {cmdArguments.Folder} didn't exist. ABulkCopy.Cmd finished.");
                    return;
                }
                await copyOut.Run(cmdArguments.Folder, cmdArguments.SearchStr);
            }
            Log.Information("ABulkCopy.Cmd finished.");
            Console.WriteLine("ABulkCopy.Cmd finished.");
        }
        catch (Exception ex)
        {
            Console.Write("Host terminated unexpectedly: ");
            Console.WriteLine(ex.FlattenMessages());
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IConfigurationRoot GetConfiguration(CmdArguments cmdArguments)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddUserSecrets("128e015d-d8ef-4ca8-ba79-5390b26c675f")
            .AddEnvironmentVariables()
            .AddInMemoryCollection(new Dictionary<string, string?> {
                ["ConnectionStrings:BulkCopy"] = cmdArguments.ConnectionString})
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

    private static ILogger ConfigureLogger(
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
        return loggerConfig.CreateLogger();
    }

    private static void ConfigureServices(
        HostApplicationBuilder builder,
        Rdbms rdbms,
        IConfigurationRoot configuration)
    {
        builder.Services.AddSingleton(configuration);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddSingleton<IFileSystem>(new FileSystem());
        builder.Services.AddSingleton<ISchemaWriter, SchemaWriter>();
        builder.Services.AddSingleton<IDataWriter, DataWriter>();
        builder.Services.AddSingleton<ITableReaderFactory, TableReaderFactory>();
        builder.Services.AddSingleton<ISelectCreator, SelectCreator>();
        builder.Services.AddSingleton<ICopyOut, CopyOut>();
        builder.Services.AddSingleton<ICopyIn, CopyIn>();
        builder.Services.AddTransient<IDataFileReader, DataFileReader>();
        builder.Services.AddSingleton<IMappingFactory, MappingFactory>();
        if (rdbms == Rdbms.Mss) builder.Services.AddMssServices();
        if (rdbms == Rdbms.Pg) builder.Services.AddPgServices();
    }

    private static HostApplicationBuilder CreateAppBuilder(
        Rdbms rdbms, IConfigurationRoot configuration)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddConfiguration(configuration);
        builder.Logging.AddSerilog();

        ConfigureServices(builder, rdbms, configuration);

        return builder;
    }
}