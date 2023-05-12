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
    [InlineData("%", 36)]
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
    public async Task TestGetTableInfo_Then_NameAndSchemaOk()
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
    }

    [Fact]
    public async Task TestGetTableInfo_Then_IdentityOk()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();

        // Act
        var tableDef = await sqlCmd.GetTableInfo("AllTypes");

        // Assert
        tableDef.Should().NotBeNull();
        tableDef!.Identity.Should().NotBeNull();
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

    [Fact]
    public async Task TestGetColumnInfo_When_DefaultValues()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();
        var tableDef = await sqlCmd.GetTableInfo("TestDefaults");
        tableDef.Should().NotBeNull();

        // Act
        var columnInfo = (await sqlCmd.GetColumnInfo(tableDef!)).ToList();

        // Assert
        columnInfo.Should().NotBeNull("because ColumnInfo shouldn't be null");
        columnInfo.Count.Should().Be(5);
        columnInfo[1].DefaultConstraint.Should().NotBeNull("because DefaultConstraint for first column shouldn't be null");
        columnInfo[1].DefaultConstraint!.Name.Should().StartWith("DF__");
        columnInfo[1].DefaultConstraint!.IsSystemNamed.Should().BeTrue();
        columnInfo[1].DefaultConstraint!.Definition.Should().Be("((0))");
        columnInfo[2].DefaultConstraint.Should().NotBeNull("because DefaultConstraint for second column shouldn't be null");
        columnInfo[2].DefaultConstraint!.Name.Should().Be("df_bulkcopy_int");
        columnInfo[2].DefaultConstraint!.IsSystemNamed.Should().BeFalse();
        columnInfo[2].DefaultConstraint!.Definition.Should().Be("((0))");
        columnInfo[3].DefaultConstraint.Should().NotBeNull("because DefaultConstraint for third column shouldn't be null");
        columnInfo[3].DefaultConstraint!.Name.Should().Be("df_num_default");
        columnInfo[3].DefaultConstraint!.IsSystemNamed.Should().BeFalse();
        columnInfo[3].DefaultConstraint!.Definition.Should().StartWith("CREATE DEFAULT");
        columnInfo[4].DefaultConstraint.Should().NotBeNull("because DefaultConstraint for fourth column shouldn't be null");
        columnInfo[4].DefaultConstraint!.Name.Should().StartWith("DF__");
        columnInfo[4].DefaultConstraint!.IsSystemNamed.Should().BeTrue();
        columnInfo[4].DefaultConstraint!.Definition.Should().Contain("getdate");
    }

    [Fact]
    public async Task TestGetPrimaryKey_When_Exists()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();
        var tableDef = await sqlCmd.GetTableInfo("ClientScope");
        tableDef.Should().NotBeNull();

        // Act
        var pk = await sqlCmd.GetPrimaryKey(tableDef!);

        // Assert
        pk.Should().NotBeNull();
        pk!.Name.Should().Be("PK_ClientScope");
        pk.ColumnNames.Count.Should().Be(2);
        pk.ColumnNames[0].Name.Should().Be("ClientId");
        pk.ColumnNames[0].Direction.Should().Be(Direction.Ascending);
        pk.ColumnNames[1].Name.Should().Be("ScopeId");
        pk.ColumnNames[1].Direction.Should().Be(Direction.Ascending);
    }

    [Fact]
    public async Task TestGetPrimaryKey_When_NotExist()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();
        var tableDef = await sqlCmd.GetTableInfo("TestDefaults");
        tableDef.Should().NotBeNull();

        // Act
        var pk = await sqlCmd.GetPrimaryKey(tableDef!);

        // Assert
        pk.Should().BeNull("because TestDefaults doesn't have a primary key");
    }

    [Fact]
    public async Task TestGetForeignKey_WhenExists()
    {
        // Arrange
        var sqlCmd = CreateASqlCommand();
        var tableDef = await sqlCmd.GetTableInfo("ClientScope");
        tableDef.Should().NotBeNull();

        // Act
        var fks = await sqlCmd.GetForeignKeys(tableDef!);

        // Assert
        var foreignKeys = fks.ToList();
        foreignKeys.Should().NotBeNull();
        foreignKeys.Count.Should().Be(2);
        foreignKeys[0].Name.Should().Be("FK_ClientScope_ConfiguredClients_ClientId");
        foreignKeys[0].ColName.Should().Be("ClientId");
        foreignKeys[0].ColumnReference.Should().Be("ClientId");
        foreignKeys[0].DeleteAction.Should().Be(DeleteAction.Cascade);
        foreignKeys[0].UpdateAction.Should().Be(UpdateAction.NoAction);
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