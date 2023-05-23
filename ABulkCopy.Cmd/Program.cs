using System.IO.Abstractions;
using ABulkCopy.Common.Reader;
using ABulkCopy.Common.Utils;
using ABulkCopy.Common.Writer;
using ASqlServer;
using ASqlServer.Column;
using ASqlServer.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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

        ConfigureLogger(configuration);

        try
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddConfiguration(configuration);
            builder.Logging.AddSerilog();

            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddSingleton<IFileSystem>(new FileSystem());
            builder.Services.AddSingleton<ISchemaWriter, SchemaWriter>();
            builder.Services.AddSingleton<IDataWriter, DataWriter>();
            builder.Services.AddSingleton<ISelectCreator, SelectCreator>();
            builder.Services.AddTransient<IMssCommand, MssCommand>();
            builder.Services.AddSingleton<IMssTableReader, MssTableReader>();
            builder.Services.AddSingleton<IMssTableSchema, MssTableSchema>();
            builder.Services.AddSingleton<IMssColumnFactory, MssColumnFactory>();

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

    private static void ConfigureLogger(IConfigurationRoot configuration)
    {
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";
        var filePath = GetLogStreamFullPath(configuration);
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File(filePath,
                outputTemplate: outputTemplate,
                fileSizeLimitBytes: 10000000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1));
        Log.Logger = loggerConfig.CreateLogger();    }

    private static string GetLogStreamFullPath(IConfiguration configuration)
    {
        var path = configuration[CmdConstants.Config.LogPathKey];
        var logFile = configuration[CmdConstants.Config.LogFileKey];

        if (string.IsNullOrWhiteSpace(path) || 
            string.IsNullOrWhiteSpace(logFile))
        {
            throw new InvalidOperationException(
                "LogStreamPath or LogStreamFile is not configured");
        }
        return path.TrimEnd('\\', '/') + @"\" + logFile;
    }
}
