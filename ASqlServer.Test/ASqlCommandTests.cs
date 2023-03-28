namespace ASqlServer.Test;

public class ASqlCommandTests
{
    private readonly ILogger _output;
    private readonly IConfiguration _configuration;

    public ASqlCommandTests(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
    }

    [Theory]
    [InlineData("%", 37)]
    [InlineData("does_not_exist", 0)]
    [InlineData("ConfiguredClients", 1)]
    [InlineData("CONFIGUREDCLIENTS", 1)]
    [InlineData("CONFIGURED%", 3)]
    [InlineData("%Sec%", 2)]
    public async Task TestGetTableNames(string searchString, int expectedCount)
    {
        var connectionString = _configuration.GetConnectionString("FromDb");
        connectionString.Should().NotBeNullOrWhiteSpace("because the connection string should be set in the user secrets file");
        var sqlCmd = new ASqlCommand(_output)
        {
            ConnectionString =  connectionString!
        };
        var tableNames = await sqlCmd.GetTableNames(searchString).ToListAsync();
        tableNames.Count.Should().Be(expectedCount, $"because there should be {expectedCount} tables returned");
    }

}