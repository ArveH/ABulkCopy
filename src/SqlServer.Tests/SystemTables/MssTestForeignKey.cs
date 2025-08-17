using ForeignKey = ABulkCopy.Common.Types.Table.ForeignKey;

namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestForeignKey(DatabaseFixture dbFixture, ITestOutputHelper output) 
    : MssTestBase(dbFixture, output)
{
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