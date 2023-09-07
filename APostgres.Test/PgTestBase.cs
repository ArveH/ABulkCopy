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

        PgContext = new PgContext(TestLoggerFactory, TestConfiguration);
    }

    //[MethodImpl(MethodImplOptions.NoInlining)]
    protected string GetName()
    {
        var st = new StackTrace();
        // Frames:
        //   0: GetName
        //   1: MoveNext
        //   2: Start
        //   3: <Should be the name of the test method>
        var sf = st.GetFrame(3);
        if (sf == null)
        {
            throw new InvalidOperationException("Stack Frame is null");
        }

        var methodName = sf.GetMethod()?.Name ?? throw new InvalidOperationException("Method is null");
        var methodName20 = methodName.Length > 20 ? methodName[4..24] : methodName;
        var machineName20 = Environment.MachineName.Length > 20 ? Environment.MachineName[..20] : Environment.MachineName;

        return machineName20 + methodName20;
    }
}