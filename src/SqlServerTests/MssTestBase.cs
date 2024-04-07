namespace SqlServerTests;

public abstract class MssTestBase
{
    protected readonly DatabaseFixture DbFixture;
    protected readonly ILogger TestLogger;
    protected readonly IMssSystemTables MssSystemTables;

    protected MssTestBase(DatabaseFixture dbFixture, ITestOutputHelper output)
    {
        DbFixture = dbFixture;
        TestLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        MssSystemTables = CreateMssSystemTables();
    }

    private IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = DbFixture.TestConfiguration.Check(TestConstants.Config.ConnectionString);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory();
        IMssSystemTables systemTables = new MssSystemTables(
            DbFixture.MssDbContext,
            colFactory, TestLogger);
        return systemTables;
    }
}