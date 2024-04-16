namespace SqlServer.Tests.SystemTables;

[Collection(nameof(DatabaseCollection))]
public class MssTestGetTableNamesSingleSchema(
    DatabaseFixture dbFixture, ITestOutputHelper output)
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
}