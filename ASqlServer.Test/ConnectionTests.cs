namespace ASqlServer.Test;

public class ConnectionTests
{
    private readonly ILogger _output;

    public ConnectionTests(ITestOutputHelper output)
    {
        _output = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();
    }

    [Fact]
    public async Task TestConnect()
    {
        var sqlCmd = new ASqlCommand(_output) { ConnectionString = "Server=.;Database=U4IDS4;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" };
        var tableNames = await sqlCmd.GetTableNames("%").ToListAsync();
        tableNames.Should().NotBeNullOrEmpty();
    }
}