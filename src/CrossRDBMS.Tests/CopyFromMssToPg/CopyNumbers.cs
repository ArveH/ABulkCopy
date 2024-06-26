namespace CrossRDBMS.Tests.CopyFromMssToPg;

[Collection(nameof(DatabaseCollection))]
public class CopyNumbers(DatabaseFixture fixture, ITestOutputHelper output) 
    : CopyMssToPgBase(fixture, output)
{
    [Fact]
    public async Task CopyInt()
    {
        var tableName = GetName(nameof(CopyFromMssToPg));
        var colValue = 12345;
        var col = new SqlServerInt(101, "col1", false);

        await TestSingleTypeAsync(
            tableName, col, colValue.ToString(), "integer");

        await ValidateValueAsync(("public", tableName), colValue);
    }
}