namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestForeignKey : PgTestBase
{
    private readonly IIdentifier _identifier;
    public PgTestForeignKey(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
        _identifier = GetIdentifier();
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
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, childTable);
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, parent1Table);
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, parent2Table);
            await ExecuteNonQueryAsync(
                $"CREATE TABLE {parent1Table}" +
                $"  (Id int NOT NULL, AnotherCol varchar(20), " + Environment.NewLine +
                $"  PRIMARY KEY (Id))");
            await ExecuteNonQueryAsync(
                $"CREATE TABLE {parent2Table}" +
                $"  (Id int NOT NULL, AnotherCol varchar(20), " + Environment.NewLine +
                $"  PRIMARY KEY (Id))");
            await ExecuteNonQueryAsync(
                $"CREATE TABLE {childTable}" +
                $"  (Id int NOT NULL, Parent1Id int, Parent2Id int, AnotherCol varchar(20), " + Environment.NewLine +
                $"  PRIMARY KEY (Id), " + Environment.NewLine +
                $"  FOREIGN KEY (Parent1Id) " + Environment.NewLine +
                $"    REFERENCES {parent1Table} (Id) " +
                $"    ON DELETE CASCADE, " + Environment.NewLine +
                $"  FOREIGN KEY (Parent2Id) " + Environment.NewLine +
                $"    REFERENCES {parent2Table} (Id) " +
                $"    ON DELETE CASCADE )");
            var tableHeader = await PgSystemTables.GetTableHeaderAsync(
                DatabaseFixture.DefaultSchemaName, childTable, CancellationToken.None);
            tableHeader.Should().NotBeNull();

            // Act
            var fks = await PgSystemTables.GetForeignKeysAsync(tableHeader!, CancellationToken.None);

            // Assert
            var foreignKeys = fks.ToList();
            foreignKeys.Should().NotBeNull();
            foreignKeys.Count.Should().Be(2);
            VerifyForeignKey(foreignKeys[0], childTable, parent1Table, _identifier.AdjustForSystemTable("Parent1Id"));
            VerifyForeignKey(foreignKeys[1], childTable, parent2Table, _identifier.AdjustForSystemTable("Parent2Id"));
        }
        finally
        {
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, childTable);
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, parent1Table);
            await DropTableAsync(DatabaseFixture.DefaultSchemaName, parent2Table);
        }
    }

    private void VerifyForeignKey(
        ForeignKey foreignKey,
        string childTable,
        string parentTable,
        string fkColName)
    {
        foreignKey.ColumnNames[0].Should().Be(fkColName);
        foreignKey.ColumnReferences[0].Should().Be(_identifier.AdjustForSystemTable("Id"));
        foreignKey.DeleteAction.Should().Be(DeleteAction.Cascade);
        foreignKey.UpdateAction.Should().Be(UpdateAction.NoAction);
    }
}