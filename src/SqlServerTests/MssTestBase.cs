using System.Diagnostics;

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
        return methodName.Length > 54 ? methodName[4..54] : methodName;
    }
}