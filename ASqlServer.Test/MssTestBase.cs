namespace ASqlServer.Test;

public abstract class MssTestBase
{
    protected readonly ILogger TestLogger;
    protected readonly IConfiguration TestConfiguration;
    protected readonly IDbContext MssDbContext;

    protected MssTestBase(ITestOutputHelper output)
    {
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        TestConfiguration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");

        MssDbContext = new MssContext()
        {
            ConnectionString = MssDbHelper.Instance.ConnectionString
        };
    }

    protected IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = TestConfiguration.GetConnectionString(TestConstants.Config.DbKey);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory(TestLogger);
        IMssSystemTables systemTables = new MssSystemTables(
            new MssContext() { ConnectionString = connectionString! },
            colFactory, TestLogger);
        return systemTables;
    }
}