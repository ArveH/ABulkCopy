namespace APostgres.Tests.SchemaReader;

public class PgSchemaReaderDateTimeTests : PgSchemaReaderBase
{
    public PgSchemaReaderDateTimeTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task ReadMssDate()
    {
        var result = await GetColFromTableDefinition(new SqlServerDate(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresDate(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssDateTime()
    {
        var result = await GetColFromTableDefinition(new SqlServerDateTime(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresTimestampTz(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssDateTime2()
    {
        var result = await GetColFromTableDefinition(new SqlServerDateTime2(1, "MyTestCol", false, 5));
        VerifyColumn(result, new PostgresTimestampTz(1, "MyTestCol", false, 5));
    }

    [Fact]
    public async Task ReadMssDateTimeOffset()
    {
        var result = await GetColFromTableDefinition(new SqlServerDateTimeOffset(1, "MyTestCol", false, 3));
        VerifyColumn(result, new PostgresTimestampTz(1, "MyTestCol", false, 3));
    }

    [Fact]
    public async Task ReadMssSmallDateTime()
    {
        var result = await GetColFromTableDefinition(new SqlServerSmallDateTime(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresTimestamp(1, "MyTestCol", false));
    }

    [Fact]
    public async Task ReadMssTime()
    {
        var result = await GetColFromTableDefinition(new SqlServerTime(1, "MyTestCol", false));
        VerifyColumn(result, new PostgresTime(1, "MyTestCol", false));
    }
}