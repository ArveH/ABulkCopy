namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestColumnInfo(DatabaseFixture dbFixture, ITestOutputHelper output) 
    : MssTestBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetColumnInfo_When_AllTypes()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync(
            $"CREATE TABLE [dbo].[{tableName}](\r\n\t[Id] [bigint] IDENTITY(1,1) NOT NULL,\r\n\t[ExactNumBigInt] [bigint] NOT NULL,\r\n\t[ExactNumInt] [int] NOT NULL,\r\n\t[ExactNumSmallInt] [smallint] NOT NULL,\r\n\t[ExactNumTinyInt] [tinyint] NOT NULL,\r\n\t[ExactNumBit] [bit] NOT NULL,\r\n\t[ExactNumMoney] [money] NOT NULL,\r\n\t[ExactNumSmallMoney] [smallmoney] NOT NULL,\r\n\t[ExactNumDecimal] [decimal](28, 3) NOT NULL,\r\n\t[ExactNumNumeric] [numeric](28, 3) NOT NULL,\r\n\t[ApproxNumFloat] [float] NOT NULL,\r\n\t[ApproxNumReal] [real] NOT NULL,\r\n\t[DTDate] [date] NOT NULL,\r\n\t[DTDateTime] [datetime] NOT NULL,\r\n\t[DTDateTime2] [datetime2](7) NOT NULL,\r\n\t[DTSmallDateTime] [smalldatetime] NOT NULL,\r\n\t[DTDateTimeOffset] [datetimeoffset](7) NOT NULL,\r\n\t[DTTime] [time](7) NOT NULL,\r\n\t[CharStrChar20] [char](20) NULL,\r\n\t[CharStrVarchar20] [varchar](20) NULL,\r\n\t[CharStrVarchar10K] [varchar](max) NULL,\r\n\t[CharStrNChar20] [nchar](20) NULL,\r\n\t[CharStrNVarchar20] [nvarchar](20) NULL,\r\n\t[CharStrNVarchar10K] [nvarchar](max) NULL,\r\n\t[BinBinary5K] [binary](5000) NULL,\r\n\t[BinVarbinary10K] [varbinary](max) NULL,\r\n\t[OtherGuid] [uniqueidentifier] NOT NULL,\r\n\t[OtherXml] [xml] NULL,\r\n CONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n))");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, CancellationToken.None)).ToList();

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
        var tableName = GetName();
        var constraintName = "Def_" + tableName;
        await CreateTableWithDefaultValuesAsync(tableName, constraintName);
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var columnInfo = (await MssSystemTables.GetTableColumnInfoAsync(tableHeader!, CancellationToken.None)).ToList();

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
        columnInfo[3].DefaultConstraint!.Name.Should().Be(constraintName);
        columnInfo[3].DefaultConstraint!.IsSystemNamed.Should().BeFalse();
        columnInfo[3].DefaultConstraint!.Definition.Should().StartWith("CREATE DEFAULT");
        columnInfo[4].DefaultConstraint.Should().NotBeNull("because DefaultConstraint for fourth column shouldn't be null");
        columnInfo[4].DefaultConstraint!.Name.Should().StartWith("DF__");
        columnInfo[4].DefaultConstraint!.IsSystemNamed.Should().BeTrue();
        columnInfo[4].DefaultConstraint!.Definition.Should().Contain("getdate");
    }

    private async Task CreateTableWithDefaultValuesAsync(
        string tableName, string constraintName)
    {
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync($"DROP DEFAULT IF EXISTS {constraintName};");
        await ExecuteNonQueryAsync($"CREATE DEFAULT {constraintName} AS 0;");
        await ExecuteNonQueryAsync(
            $"CREATE TABLE {tableName}(\r\n" +
            $"  id INTEGER,\r\n" +
            $"  int1 INT NOT NULL DEFAULT 0,\r\n" +
            $"  int2 INT NOT NULL CONSTRAINT df_bulkcopy_int DEFAULT 0,\r\n" +
            $"  int3 INT NOT NULL,\r\n" +
            $"  date1 DATETIME2 NOT NULL DEFAULT GETDATE());");
        await ExecuteNonQueryAsync($"exec sp_bindefault '{constraintName}', '{tableName}.int3'");

    }
}