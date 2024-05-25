using ForeignKey = ABulkCopy.Common.Types.Table.ForeignKey;

namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssSystemTablesTests(DatabaseFixture dbFixture, ITestOutputHelper output) 
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

    [Fact]
    public async Task TestGetPrimaryKey_When_Exists()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync($"CREATE TABLE [{tableName}]([Key1] int NOT NULL, [Key2] int NOT NULL, [AnotherCol] nvarchar(20), \r\nCONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Key1] ASC,\r\n\t[Key2] ASC\r\n))");

        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().NotBeNull();
        pk!.Name.Should().Be($"PK_{tableName}");
        pk.ColumnNames.Count.Should().Be(2);
        pk.ColumnNames[0].Name.Should().Be("Key1");
        pk.ColumnNames[0].Direction.Should().Be(Direction.Ascending);
        pk.ColumnNames[1].Name.Should().Be("Key2");
        pk.ColumnNames[1].Direction.Should().Be(Direction.Ascending);
    }

    [Fact]
    public async Task TestGetPrimaryKey_When_NotExist()
    {
        // Arrange
        var tableName = GetName();
        await DropTableAsync(tableName);
        await ExecuteNonQueryAsync($"CREATE TABLE [{tableName}]([Key1] int NOT NULL, [Key2] int NOT NULL, [AnotherCol] nvarchar(20))");
        var tableHeader = await MssSystemTables.GetTableHeaderAsync(
            "dbo", tableName, CancellationToken.None);
        tableHeader.Should().NotBeNull();

        // Act
        var pk = await MssSystemTables.GetPrimaryKeyAsync(tableHeader!, CancellationToken.None);

        // Assert
        pk.Should().BeNull($"because {tableName} doesn't have a primary key");
    }

    [Fact]
    public async Task TestGetForeignKey_WhenExists()
    {
        var parent1Table = GetName() + "_Parent1";
        var parent2Table = GetName() + "_Parent2";
        var childTable = GetName() + "_Child";
        try
        {
            // Arrange
            await DropTableAsync(childTable);
            await DropTableAsync(parent1Table);
            await DropTableAsync(parent2Table);
            await ExecuteNonQueryAsync(
                $"CREATE TABLE [{parent1Table}]" +
                $"  ([Id] int NOT NULL, [AnotherCol] nvarchar(20), \r\n" +
                $"  CONSTRAINT [PK_{parent1Table}] PRIMARY KEY CLUSTERED ([Id] ASC))");
            await ExecuteNonQueryAsync(
                $"CREATE TABLE [{parent2Table}]" +
                $"  ([Id] int NOT NULL, [AnotherCol] nvarchar(20), \r\n" +
                $"  CONSTRAINT [PK_{parent2Table}] PRIMARY KEY CLUSTERED ([Id] ASC))");
            await ExecuteNonQueryAsync(
                $"CREATE TABLE [{childTable}]" +
                $"  ([Id] int NOT NULL, Parent1Id int, Parent2Id int, [AnotherCol] nvarchar(20), \r\n" +
                $"  CONSTRAINT [PK_{childTable}] PRIMARY KEY CLUSTERED ([Id] ASC), \r\n" +
                $"  CONSTRAINT [FK_{childTable}_{parent1Table}_Parent1Id] " +
                $"    FOREIGN KEY ([Parent1Id]) \r\n\t" +
                $"    REFERENCES [{parent1Table}] ([Id]) " +
                $"    ON DELETE CASCADE, \r\n" +
                $"  CONSTRAINT [FK_{childTable}_{parent2Table}_Parent2Id] " +
                $"    FOREIGN KEY ([Parent2Id]) \r\n\t" +
                $"    REFERENCES [{parent2Table}] ([Id]) " +
                $"    ON DELETE CASCADE )");
            var tableHeader = await MssSystemTables.GetTableHeaderAsync(
                "dbo", childTable, CancellationToken.None);
            tableHeader.Should().NotBeNull();

            // Act
            var fks = await MssSystemTables.GetForeignKeysAsync(tableHeader!, CancellationToken.None);

            // Assert
            var foreignKeys = fks.ToList();
            foreignKeys.Should().NotBeNull();
            foreignKeys.Count.Should().Be(2);
            VerifyForeignKey(foreignKeys[0], childTable, parent1Table, "Parent1Id");
            VerifyForeignKey(foreignKeys[1], childTable, parent2Table, "Parent2Id");
        }
        finally
        {
            await DropTableAsync(childTable);
            await DropTableAsync(parent1Table);
            await DropTableAsync(parent2Table);
        }
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

    private static void VerifyForeignKey(
        ForeignKey foreignKey,
        string childTable,
        string parentTable,
        string fkColName)
    {
        foreignKey.ConstraintName.Should().Be($"FK_{childTable}_{parentTable}_{fkColName}");
        foreignKey.ColumnNames[0].Should().Be(fkColName);
        foreignKey.ColumnReferences[0].Should().Be("Id");
        foreignKey.DeleteAction.Should().Be(DeleteAction.Cascade);
        foreignKey.UpdateAction.Should().Be(UpdateAction.NoAction);
    }
}