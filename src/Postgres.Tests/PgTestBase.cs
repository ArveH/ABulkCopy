namespace Postgres.Tests;

public class PgTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly Microsoft.Extensions.Logging.ILoggerFactory TestLoggerFactory;
    protected readonly DatabaseFixture DbFixture;

    protected PgTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
    {
        DbFixture = dbFixture;
        const string outputTemplate =
            "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message}    {Timestamp:yyyy-MM-dd }{Properties}{NewLine}{Exception}{NewLine}";
        var loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.TestOutput(output);

        var fileFullPath = DbFixture.Configuration[Constants.Config.LogFile];
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
        var methodName20 = methodName.Length > 24 ? methodName[4..24] : methodName;
        var machineName20 = Environment.MachineName.Length > 20 ? Environment.MachineName[..20] : Environment.MachineName;

        return machineName20 + methodName20;
    }

    protected static Identifier GetIdentifier(
        Dictionary<string, string?> appSettings,
        Rdbms rdbms = Rdbms.Pg)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettings)
            .Build();
        var dbContextMock = new Mock<IDbContext>();
        dbContextMock.Setup(x => x.Rdbms).Returns(rdbms);
        var identifier = new Identifier(config, dbContextMock.Object);
        return identifier;
    }

    protected static IQueryBuilderFactory GetQueryBuilderFactory(
        Dictionary<string, string?> appSettings)
    {
        var qbFactoryMock = new Mock<IQueryBuilderFactory>();
        qbFactoryMock
            .Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                GetIdentifier(appSettings)));
        return qbFactoryMock.Object;
    }

    protected IPgCmd GetPgCmd(Dictionary<string, string?>? appSettings = null)
    {
        appSettings ??= new()
        {
            { Constants.Config.AddQuotes, "true" },
        };
        appSettings.Add(
            Constants.Config.PgConnectionString,
            DbFixture.Configuration.SafeGet(Constants.Config.PgConnectionString));
        return new ABulkCopy.APostgres.PgCmd(
            DbFixture.PgContext,
            GetQueryBuilderFactory(appSettings),
            TestLogger);
    }

    protected IPgSystemTables GetPgSystemTables(Dictionary<string, string?>? appSettings = null)
    {
        appSettings ??= new()
        {
            { Constants.Config.AddQuotes, "true" },
        };
        return new PgSystemTables(
            DbFixture.PgContext,
            GetQueryBuilderFactory(appSettings),
            GetIdentifier(appSettings), TestLogger);
    }
}