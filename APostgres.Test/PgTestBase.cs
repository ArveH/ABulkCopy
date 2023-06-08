namespace APostgres.Test;

public class PgTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly Microsoft.Extensions.Logging.ILoggerFactory TestLoggerFactory;
    protected readonly IConfiguration TestConfiguration;
    protected readonly IPgContext PgContext;

    protected PgTestBase(ITestOutputHelper output)
    {
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";
        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
        var loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.TestOutput(output);

        var fileFullPath = TestConfiguration["LogFile"];
        if (!string.IsNullOrWhiteSpace(fileFullPath))
        {
            loggerConfig
                .WriteTo.File(fileFullPath,
                    outputTemplate: outputTemplate,
                    fileSizeLimitBytes: 10000000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1));
        }

        TestLogger = loggerConfig.CreateLogger();

        TestLoggerFactory = new Microsoft.Extensions.Logging.LoggerFactory().AddSerilog(TestLogger);

        PgContext = new PgContext(TestLoggerFactory)
        {
            ConnectionString = PgDbHelper.Instance.ConnectionString
        };
    }
}