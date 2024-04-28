namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestGetTableNamesMultipleSchemas(
    DatabaseFixture dbFixture, ITestOutputHelper output)
    : MssTestGetTableNamesBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetTableNames_When_SameTableInDifferentSchemas()
    {
        var guid = Guid.NewGuid().ToString("N");
        var testTableName = "T_" + guid;
        var tableList = await TestGetTableNamesAsync(guid, "", testTableName);
        tableList.Count().Should().Be(2);
        tableList[0].schemaName.Should().Be("dbo");
        tableList[0].tableName.Should().Be(testTableName);
        tableList[1].schemaName.Should().Be(DatabaseFixture.TestSchemaName);
        tableList[1].tableName.Should().Be(testTableName);
    }

    [Fact]
    public async Task TestGetTableNames_When_FilterOnSchema()
    {
        var guid = Guid.NewGuid().ToString("N");
        var testTableName = "T_" + guid;
        var tableList = await TestGetTableNamesAsync(guid, "dbo", testTableName);
        tableList.Count().Should().Be(1);
        tableList[0].schemaName.Should().Be("dbo");
        tableList[0].tableName.Should().Be(testTableName);
    }

    [Fact]
    public async Task TestGetTableNames_When_TableNotExist()
    {
        var guid = Guid.NewGuid().ToString("N");
        var tableList = await TestGetTableNamesAsync(guid, "dbo", "DoesNotExist");
        tableList.Count().Should().Be(0);
    }

    [Fact]
    public async Task TestGetTableNames_When_AllTables()
    {
        var guid = Guid.NewGuid().ToString("N");
        var tableList = await TestGetTableNamesAsync(guid, "dbo", "%");
        tableList.Count().Should().BeGreaterThanOrEqualTo(3);
    }

    private async Task<List<SchemaTableTuple>> TestGetTableNamesAsync(
        string guid, string schemaName, string searchString)
    {
        var testTableName = "T_" + guid;
        var testTables = new List<SchemaTableTuple>
        {
            new(DatabaseFixture.TestSchemaName, testTableName),
            new("dbo", testTableName),
            new("dbo", "T_ExtraTable"),
        };
        return await CreateAndCleanupTablesAsync(testTables, async () =>
        {
            // Act
            var result = await MssSystemTables.GetFullTableNamesAsync(
                schemaName, searchString, CancellationToken.None);

            return result.ToList();
        });
    }
}