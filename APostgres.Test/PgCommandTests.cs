namespace APostgres.Test;

public class PgCommandTests
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public PgCommandTests(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
    }

    [Fact]
    public async Task TestConnect()
    {
        var connectionString = _configuration.GetConnectionString(TestConstants.Config.DbKey);
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set");
        var pgCmd = new PgCommand(
            new PgContext { ConnectionString = connectionString! },
            _logger);

        var res = await pgCmd.Connect();
        res.Should().BeTrue("because the connection should be successful");
    }
}