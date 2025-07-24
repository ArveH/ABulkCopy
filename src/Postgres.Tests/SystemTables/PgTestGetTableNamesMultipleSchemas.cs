namespace Postgres.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class PgTestGetTableNamesMultipleSchemas(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : PgTestGetTableNamesBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetTableNames_When_SameTableInDifferentSchemas()
    {
        var guid = Guid.NewGuid().ToString("N");
        var testTableName = "T_" + guid;
        var tableList = await TestGetTableNamesAsync(guid, "", testTableName);
        tableList.Count.Should().Be(2);
        tableList.Count(t => t.tableName.Equals(testTableName, StringComparison.InvariantCultureIgnoreCase)).Should().Be(2);
        tableList.Count(t => t.schemaName == DatabaseFixture.DefaultSchemaName).Should().Be(1);
        tableList.Count(t => t.schemaName == DatabaseFixture.TestSchemaName).Should().Be(1);
    }

    [Fact]
    public async Task TestGetTableNames_When_FilterOnSchema()
    {
        var guid = Guid.NewGuid().ToString("N");
        var testTableName = "T_" + guid;
        var tableList = await TestGetTableNamesAsync(guid, DatabaseFixture.DefaultSchemaName, testTableName);
        tableList.Count.Should().Be(1);
        tableList[0].schemaName.Should().BeEquivalentTo(DatabaseFixture.DefaultSchemaName);
        tableList[0].tableName.Should().BeEquivalentTo(testTableName);
    }

    [Fact]
    public async Task TestGetTableNames_When_TableNotExist()
    {
        var guid = Guid.NewGuid().ToString("N");
        var tableList = await TestGetTableNamesAsync(guid, DatabaseFixture.DefaultSchemaName, "DoesNotExist");
        tableList.Count.Should().Be(0);
    }

    [Fact]
    public async Task TestGetTableNames_When_AllTables()
    {
        var guid = Guid.NewGuid().ToString("N");
        var tableList = await TestGetTableNamesAsync(guid, DatabaseFixture.DefaultSchemaName, "%");
        tableList.Count.Should().BeGreaterThanOrEqualTo(3);
    }

    private async Task<List<SchemaTableTuple>> TestGetTableNamesAsync(
        string guid, string schemaName, string searchString)
    {
        var testTableName = "T_" + guid;
        var testTables = new List<SchemaTableTuple>
        {
            new(DatabaseFixture.TestSchemaName, testTableName),
            new(DatabaseFixture.DefaultSchemaName, testTableName),
            new(DatabaseFixture.DefaultSchemaName, "T_ExtraTable"),
        };
        return await CreateAndCleanupTablesAsync(testTables, async () =>
        {
            // Act
            var result = await PgSystemTables.GetFullTableNamesAsync(
                schemaName, searchString, CancellationToken.None);

            return result.ToList();
        });
    }
}