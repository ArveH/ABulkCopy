namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestGetTableNamesSingleSchema(
    DatabaseFixture dbFixture,
    ITestOutputHelper output)
    : MssTestGetTableNamesBase(dbFixture, output)
{
    [Fact]
    public async Task TestGetTableNames_When_NotExists()
    {
        var guid = Guid.NewGuid().ToString("N");
        await TestGetTableNamesAsync(guid, "does_not_exist", 0);
    }

    [Fact]
    public async Task TestGetTableNames_When_ExactName()
    {
        var guid = Guid.NewGuid().ToString("N");
        await TestGetTableNamesAsync(guid, "T_" + guid + "1", 1);
    }

    [Fact]
    public async Task TestGetTableNames_When_AllUpperCase()
    {
        var guid = Guid.NewGuid().ToString("N");
        await TestGetTableNamesAsync(guid, "T_" + guid.ToUpper() + "1", 1);
    }

    [Fact]
    public async Task TestGetTableNames_When_EndIsWild()
    {
        var guid = Guid.NewGuid().ToString("N");
        await TestGetTableNamesAsync(guid, "T_" + guid.ToUpper() + "%", 3);
    }

    [Fact]
    public async Task TestGetTableNames_When_StartAndEndIsWild()
    {
        var guid = Guid.NewGuid().ToString("N");
        await TestGetTableNamesAsync(guid, "%" + guid + "%", 3);
    }

    protected async Task TestGetTableNamesAsync(
        string guid, string searchString, int expectedCount)
    {
        var testTables = new List<SchemaTableTuple>
        {
            new("dbo", "T_" + guid + "1"),
            new("dbo", "T_" + guid + "2"),
            new("dbo", "T_" + guid + "3"),
            new("dbo", "T_MyNewData")
        };

        var tableNames = await CreateAndCleanupTablesAsync(testTables, async () =>
            (await MssSystemTables.GetFullTableNamesAsync(
                "dbo",
                searchString,
                CancellationToken.None)).ToList());
        
        // Assert
        tableNames.Count().Should().Be(expectedCount);
    }
}