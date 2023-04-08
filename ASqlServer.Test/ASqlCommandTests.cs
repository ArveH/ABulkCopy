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
    [InlineData("%", 34)]
    [InlineData("does_not_exist", 0)]
    [InlineData("ConfiguredClients", 1)]
    [InlineData("CONFIGUREDCLIENTS", 1)]
    [InlineData("CONFIGURED%", 3)]
    [InlineData("%Sec%", 2)]
    public async Task TestGetTableNames(string searchString, int expectedCount)
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();

        // Act
        var tableNames = await sqlCmd.GetTableNames(searchString);

        // Assert
        tableNames.Count().Should().Be(expectedCount, $"because there should be {expectedCount} tables returned");
    }

    [Fact]
    public async Task TestGetTableInfo()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();

        // Act
        var tableDef = await sqlCmd.GetTableInfo("AllTypes");

        // Assert
        tableDef.Should().NotBeNull();
        tableDef!.Id.Should().BeGreaterThan(0);
        tableDef.Name.Should().Be("AllTypes");
        tableDef.Schema.Should().Be("dbo");
        tableDef.Location.Should().Be("PRIMARY");
        tableDef.Identity.Should().NotBeNull();
        tableDef.Identity!.Seed.Should().Be(1);
        tableDef.Identity.Increment.Should().Be(1);
    }

    [Fact]
    public async Task TestGetColumnInfo_When_AllTypes()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();
        var tableDef = await sqlCmd.GetTableInfo("AllTypes");
        tableDef.Should().NotBeNull();

        // Act
        var columnInfo = (await sqlCmd.GetColumnInfo(tableDef!)).ToList();

        // Assert
        columnInfo.Should().NotBeNull();
        columnInfo.Count.Should().Be(28);
        columnInfo[0].Name.Should().Be("Id");
        columnInfo[0].DataType.Should().Be("bigint");
        columnInfo[0].IsNullable.Should().BeFalse();
        columnInfo[0].Identity.Should().NotBeNull();
        columnInfo[0].Identity!.Seed.Should().Be(1);
        columnInfo[0].Identity!.Increment.Should().Be(1);
        columnInfo[9].Precision.Should().Be(28, "because decimal column has precision 28");
        columnInfo[9].Scale.Should().Be(3, "because decimal column has scale 3");
        columnInfo[23].DataType.Should().Be("nvarchar");
        columnInfo[23].Length.Should().Be(-1, "because we are dealing with nvarchar(max)");
        columnInfo[23].Collation.Should().Be("SQL_Latin1_General_CP1_CI_AS");
        columnInfo[23].IsNullable.Should().BeTrue("because column 23 is nullable");

    }

    private IASqlCommand CreateASqlCommand()
    {
        var connectionString = _configuration.GetConnectionString("FromDb");
        connectionString.Should()
            .NotBeNullOrWhiteSpace("because the connection string should be set in the user secrets file");
        IASqlCommand sqlCmd = new ASqlCommand(_output)
        {
            ConnectionString = connectionString!
        };
        return sqlCmd;
    }
}