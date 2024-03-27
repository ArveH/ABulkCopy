namespace SqlServer.DbTest;

public abstract class MssSystemTablesTestBase : MssTestBase
{
    protected readonly IMssSystemTables MssSystemTables;

    protected MssSystemTablesTestBase(ITestOutputHelper output) 
        : base(output)
    {
        MssSystemTables = CreateMssSystemTables();
    }

    private IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = TestConfiguration.Check(TestConstants.Config.ConnectionString);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory();
        IMssSystemTables systemTables = new MssSystemTables(
            MssDbContext,
            colFactory, TestLogger);
        return systemTables;
    }
}