namespace ASqlServer.Test;

public class MssSystemTablesTests
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public MssSystemTablesTests(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _configuration = new ConfigHelper().GetConfiguration("128e015d-d8ef-4ca8-ba79-5390b26c675f");
    }

    [Theory]
    [InlineData("does_not_exist", 0)]
    [InlineData("ConfiguredClients", 1)]
    [InlineData("CONFIGUREDCLIENTS", 1)]
    [InlineData("CONFIGURED%", 3)]
    [InlineData("%Sec%", 2)]
    public async Task TestGetTableNames(string searchString, int expectedCount)
    {
        // Arrange
        var systemTables = CreateMssSystemTables();

        // Act
        var tableNames = await systemTables.GetTableNames(searchString);

        // Assert
        tableNames.Count().Should().Be(expectedCount, $"because there should be {expectedCount} tables returned");
    }

    [Fact]
    public async Task TestGetTableHeader_Then_NameAndSchemaOk()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();

        // Act
        var tableHeader = await systemTables.GetTableHeader("AllTypes");

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be("AllTypes");
        tableHeader.Schema.Should().Be("dbo");
    }

    [Fact]
    public async Task TestGetTableHeader_Then_IdentityOk()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();

        // Act
        var tableHeader = await systemTables.GetTableHeader("AllTypes");

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Identity.Should().NotBeNull();
        tableHeader.Identity!.Seed.Should().Be(1);
        tableHeader.Identity.Increment.Should().Be(1);
    }

    [Fact]
    public async Task TestGetColumnInfo_When_AllTypes()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("AllTypes");
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await systemTables.GetColumnInfo(tableHeader!)).ToList();

        // Assert
        columnInfo.Should().NotBeNull();
        columnInfo.Count.Should().Be(28);
        columnInfo[0].Name.Should().Be("Id");
        columnInfo[0].Type.Should().Be(ColumnType.BigInt);
        columnInfo[0].IsNullable.Should().BeFalse();
        columnInfo[0].Identity.Should().NotBeNull();
        columnInfo[0].Identity!.Seed.Should().Be(1);
        columnInfo[0].Identity!.Increment.Should().Be(1);
        columnInfo[9].Precision.Should().Be(28, "because decimal column has precision 28");
        columnInfo[9].Scale.Should().Be(3, "because decimal column has scale 3");
        columnInfo[23].Type.Should().Be(ColumnType.NVarChar);
        columnInfo[23].Length.Should().Be(-1, "because we are dealing with nvarchar(max)");
        columnInfo[23].Collation.Should().Be("SQL_Latin1_General_CP1_CI_AS");
        columnInfo[23].IsNullable.Should().BeTrue("because column 23 is nullable");

    }

    [Fact]
    public async Task TestGetColumnInfo_When_DefaultValues()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("TestDefaults");
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await systemTables.GetColumnInfo(tableHeader!)).ToList();

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
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("ClientScope");
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await systemTables.GetPrimaryKey(tableHeader!);

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
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("TestDefaults");
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await systemTables.GetPrimaryKey(tableHeader!);

        // Assert
        pk.Should().BeNull("because TestDefaults doesn't have a primary key");
    }

    [Fact]
    public async Task TestGetForeignKey_WhenExists()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("ClientScope");
        tableHeader.Should().NotBeNull();

        // Act
        var fks = await systemTables.GetForeignKeys(tableHeader!);

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

    [Fact]
    public async Task TestGetIndexes_When_OneIndexOneColumn()
    {
        // Arrange
        var systemTables = CreateMssSystemTables();
        var tableHeader = await systemTables.GetTableHeader("ConfiguredClients");
        tableHeader.Should().NotBeNull();

        // Act
        var indexes = (await systemTables.GetIndexes(tableHeader!)).ToList();

        // Assert
        indexes.Should().NotBeNull();
        indexes.Count.Should().Be(1);
        indexes[0].Header.Id.Should().Be(2);
        indexes[0].Header.TableId.Should().BeGreaterThan(0);
        indexes[0].Header.Name.Should().Be("IX_ConfiguredClients_OwnerTenant");
        indexes[0].Header.IsUnique.Should().BeFalse();
        indexes[0].Header.Type.Should().Be(IndexType.NonClustered);
        indexes[0].Header.Location.Should().Be("PRIMARY");
    }

    private IMssSystemTables CreateMssSystemTables()
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