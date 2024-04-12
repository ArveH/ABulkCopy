namespace SqlServerTests;

[Collection(nameof(DatabaseCollection))]
public class MssSystemTablesTests : MssTestBase
{
    private CancellationTokenSource _cts = new();

    public MssSystemTablesTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Theory]
    [InlineData("does_not_exist", 0)]
    [InlineData("ConfiguredClients", 1)]
    [InlineData("CONFIGUREDCLIENTS", 1)]
    [InlineData("CONFIGURED%", 3)]
    [InlineData("%Sec%", 2)]
    public async Task TestGetTableNames(string searchString, int expectedCount)
    {
        // Act
        var tableNames = await MssSystemTables.GetTableNamesAsync(searchString, _cts.Token);

        // Assert
        tableNames.Count().Should().Be(expectedCount, $"because there should be {expectedCount} tables returned");
    }

    [Fact]
    public async Task TestGetTableHeader_Then_NameAndSchemaOk()
    {
        // Act
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("AllTypes", _cts.Token);

        // Assert
        tableHeader.Should().NotBeNull();
        tableHeader!.Id.Should().BeGreaterThan(0);
        tableHeader.Name.Should().Be("AllTypes");
        tableHeader.Schema.Should().Be("dbo");
    }

    [Fact]
    public async Task TestGetTableHeader_Then_IdentityOk()
    {
        // Act
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("AllTypes", _cts.Token);

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
        await DbFixture.ExecuteNonQuery(
            "CREATE TABLE [dbo].[AllTypes](\r\n\t[Id] [bigint] IDENTITY(1,1) NOT NULL,\r\n\t[ExactNumBigInt] [bigint] NOT NULL,\r\n\t[ExactNumInt] [int] NOT NULL,\r\n\t[ExactNumSmallInt] [smallint] NOT NULL,\r\n\t[ExactNumTinyInt] [tinyint] NOT NULL,\r\n\t[ExactNumBit] [bit] NOT NULL,\r\n\t[ExactNumMoney] [money] NOT NULL,\r\n\t[ExactNumSmallMoney] [smallmoney] NOT NULL,\r\n\t[ExactNumDecimal] [decimal](28, 3) NOT NULL,\r\n\t[ExactNumNumeric] [numeric](28, 3) NOT NULL,\r\n\t[ApproxNumFloat] [float] NOT NULL,\r\n\t[ApproxNumReal] [real] NOT NULL,\r\n\t[DTDate] [date] NOT NULL,\r\n\t[DTDateTime] [datetime] NOT NULL,\r\n\t[DTDateTime2] [datetime2](7) NOT NULL,\r\n\t[DTSmallDateTime] [smalldatetime] NOT NULL,\r\n\t[DTDateTimeOffset] [datetimeoffset](7) NOT NULL,\r\n\t[DTTime] [time](7) NOT NULL,\r\n\t[CharStrChar20] [char](20) NULL,\r\n\t[CharStrVarchar20] [varchar](20) NULL,\r\n\t[CharStrVarchar10K] [varchar](max) NULL,\r\n\t[CharStrNChar20] [nchar](20) NULL,\r\n\t[CharStrNVarchar20] [nvarchar](20) NULL,\r\n\t[CharStrNVarchar10K] [nvarchar](max) NULL,\r\n\t[BinBinary5K] [binary](5000) NULL,\r\n\t[BinVarbinary10K] [varbinary](max) NULL,\r\n\t[OtherGuid] [uniqueidentifier] NOT NULL,\r\n\t[OtherXml] [xml] NULL,\r\n CONSTRAINT [PK_AllTypes] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n))");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("AllTypes", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, _cts.Token)).ToList();

        // Assert
        columnInfo.Should().NotBeNull();
        columnInfo.Count.Should().Be(28);
        columnInfo[0].Name.Should().Be("Id");
        columnInfo[0].Type.Should().Be(MssTypes.BigInt);
        columnInfo[0].IsNullable.Should().BeFalse();
        columnInfo[0].Identity.Should().NotBeNull();
        columnInfo[0].Identity!.Seed.Should().Be(1);
        columnInfo[0].Identity!.Increment.Should().Be(1);
        columnInfo[9].Precision.Should().Be(28, "because decimal column has precision 28");
        columnInfo[9].Scale.Should().Be(3, "because decimal column has scale 3");
        columnInfo[23].Type.Should().Be(MssTypes.NVarChar);
        columnInfo[23].Length.Should().Be(-1, "because we are dealing with nvarchar(max)");
        columnInfo[23].Collation.Should().Be("SQL_Latin1_General_CP1_CI_AS");
        columnInfo[23].IsNullable.Should().BeTrue("because column 23 is nullable");

    }

    [Fact]
    public async Task TestGetColumnInfo_When_DefaultValues()
    {
        // Arrange
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("TestDefaults", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, _cts.Token)).ToList();

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
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("ClientScope", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, _cts.Token);

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
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("TestDefaults", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, _cts.Token);

        // Assert
        pk.Should().BeNull("because TestDefaults doesn't have a primary key");
    }

    [Fact]
    public async Task TestGetForeignKey_WhenExists()
    {
        // Arrange
        var tableHeader = await MssSystemTables.GetTableHeaderAsync("ClientScope", _cts.Token);
        tableHeader.Should().NotBeNull();

        // Act
        var fks = await MssSystemTables.GetForeignKeysAsync(tableHeader!, _cts.Token);

        // Assert
        var foreignKeys = fks.ToList();
        foreignKeys.Should().NotBeNull();
        foreignKeys.Count.Should().Be(2);
        foreignKeys[0].ConstraintName.Should().Be("FK_ClientScope_ConfiguredClients_ClientId");
        foreignKeys[0].ColumnNames[0].Should().Be("ClientId");
        foreignKeys[0].ColumnReferences[0].Should().Be("ClientId");
        foreignKeys[0].DeleteAction.Should().Be(DeleteAction.Cascade);
        foreignKeys[0].UpdateAction.Should().Be(UpdateAction.NoAction);
    }
}