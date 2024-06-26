namespace CrossRDBMS.Tests.CopyFromMssToPg;

[Collection(nameof(DatabaseCollection))]
public class CopyMisc : CopyMssToPgBase
{
    public CopyMisc(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        PgArguments = ParamHelper.GetInPg(
            fixture.PgConnectionString,
            mappingsFile: "mymappings.json");
    }

    [Fact]
    public async Task CopyBit_To_Boolean()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = 1;
        var col = new SqlServerBit(101, "col1", false);
        MapTo("boolean");

        await TestSingleTypeAsync(
            tableName, col, colValue.ToString(), "boolean");

        await ValidateValueAsync(("public", tableName), true);
    }

    [Fact]
    public async Task CopyBit_To_SmallInt()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = 1;
        var col = new SqlServerBit(101, "col1", false);
        MapTo("smallint");

        await TestSingleTypeAsync(
            tableName, col, colValue.ToString(), "smallint");

        await ValidateValueAsync(("public", tableName), (short)colValue);
    }

    [Fact]
    public async Task CopyBit_To_Int()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = 1;
        var col = new SqlServerBit(101, "col1", false);
        MapTo("int");

        await TestSingleTypeAsync(
            tableName, col, colValue.ToString(), "integer");

        await ValidateValueAsync(("public", tableName), colValue);
    }

    private void MapTo(string mapTo)
    {
        DummyFileSystem.AddFile("mymappings.json", GetMappingFile(
            $"    \"bit\": \"{mapTo}\"\r\n"));
    }
}