namespace Postgres.Tests.PgCmd;

[Collection(nameof(DatabaseCollection))]
public class PgSystemTablesTest(DatabaseFixture dbFixture, ITestOutputHelper output)
    : PgTestBase(dbFixture, output)
{
    [Theory]
    [InlineData(20, 20)]
    [InlineData(64, 20)]
    [InlineData(20, 64)]
    [InlineData(64, 64)]
    public async Task TestResetIdentityColumn(int tableNameLength, int colNameLength)
    {
        var tableName = "tbl".PadRight(tableNameLength - 3, 'z');
        var colName = "col".PadRight(colNameLength - 3, 'z');
        try
        {
            // Arrange
            var systemTables = GetPgSystemTables();
            await CreateTableWithIdentityColumn(tableName, colName, 100);
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve3')");

            // Act
            await systemTables.ResetIdentityAsync(tableName, colName, CancellationToken.None);

            // Assert
            var identityValues = (await DbFixture.SelectColumn<long>(("public", tableName), colName)).ToList();
            identityValues.Count.Should().Be(3);
            identityValues.Should().Contain(120);
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }

    private async Task CreateTableWithIdentityColumn(
        string tableName, string colName, int seed)
    {
        var inputDefinition = PgTestData.GetEmpty(("public", tableName));
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
        await DbFixture.DropTable(("public", tableName));
        await DbFixture.CreateTable(inputDefinition);
        await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve1')");
        await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve2')");
    }
}