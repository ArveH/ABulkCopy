namespace ASqlServer.Test;

public class ASqlCommandTests
{
    private readonly ILogger _output;

    public ASqlCommandTests(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();
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
        var sqlCmd = new ASqlCommand(_output) { ConnectionString = "Server=.;Database=U4IDS4;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" };
        var tableNames = await sqlCmd.GetTableNames(searchString).ToListAsync();
        tableNames.Count.Should().Be(expectedCount, $"because there should be {expectedCount} tables returned");
    }

}