namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestResetIdentityColumn(DatabaseFixture dbFixture, ITestOutputHelper output)
    : PgTestBase(dbFixture, output)
{
    [Theory]
    [InlineData(20, 20, false)]
    [InlineData(64, 20, false)]
    [InlineData(20, 64, false)]
    [InlineData(64, 64, false)]
    [InlineData(20, 20, true)]
    [InlineData(64, 64, true)]
    public async Task TestResetIdentityColumn(int tableNameLength, int colNameLength, bool isCaseSensitive)
    {
        var tableName = (isCaseSensitive ? "Tbl" : "tbl").PadRight(tableNameLength - 3, 'z');
        var colName = "col".PadRight(colNameLength - 3, 'z');
        SchemaTableTuple st = ("public", tableName, isCaseSensitive);
        var quotedTableName = isCaseSensitive ? $"\"{tableName}\"" : tableName;
        try
        {
            // Arrange
            var systemTables = CreatePgSystemTables();
            await CreateTableWithIdentityColumn(st, quotedTableName, colName, 100);
            await DbFixture.ExecuteNonQuery($"insert into {quotedTableName} (Name) values ('Arve3')");

            // Act
            await systemTables.ResetIdentityAsync(st, colName, CancellationToken.None);

            // Assert
            var identityValues = (await DbFixture.SelectColumn<long>(st, colName)).ToList();
            identityValues.Count.Should().Be(3);
            identityValues.Should().Contain(120);
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }

    private async Task CreateTableWithIdentityColumn(
        SchemaTableTuple st, string quotedTableName, string colName, int seed)
    {
        var inputDefinition = PgTestData.GetEmpty(st);
        inputDefinition.Header.Identity = new Identity
        {
            Increment = 10,
            Seed = seed
        };
        var identityCol = new PostgresBigInt(1, colName, false)
        {
            Identity = inputDefinition.Header.Identity
        };
        inputDefinition.Columns.Add(identityCol);
        inputDefinition.Columns.Add(new PostgresVarChar(2, "Name", true, 100));
        await DbFixture.DropTable(st);
        await DbFixture.CreateTable(inputDefinition);
        await DbFixture.ExecuteNonQuery($"insert into {quotedTableName} (Name) values ('Arve1')");
        await DbFixture.ExecuteNonQuery($"insert into {quotedTableName} (Name) values ('Arve2')");
    }
}
