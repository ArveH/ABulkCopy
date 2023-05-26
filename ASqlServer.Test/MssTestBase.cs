namespace ASqlServer.Test;

public abstract class MssTestBase
{
    protected readonly ILogger _logger;
    protected readonly IConfiguration _configuration;
    protected readonly IDbContext MssDbContext;

    protected MssTestBase(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");

        MssDbContext = new MssContext()
        {
            ConnectionString = MssDbHelper.Instance.ConnectionString
        };
    }

    protected IMssSystemTables CreateMssSystemTables()
    {
        var connectionString = _configuration.GetConnectionString(TestConstants.Config.DbKey);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        IMssColumnFactory colFactory = new MssColumnFactory(_logger);
        IMssSystemTables systemTables = new MssSystemTables(
            new MssContext() { ConnectionString = connectionString! },
            colFactory, _logger);
        return systemTables;
    }
}