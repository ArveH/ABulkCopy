namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestTimestampTz : PgColumnTypeTestBase
{
    public TestTimestampTz(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestTimestampTz_When_Utc()
    {
        await TestTimestampTzRoundTrip(GetName(), "2023-06-23T11:12:13.4567890Z",
            new DateTimeOffset(2023, 6, 23, 11, 12, 13, 456, 789, TimeSpan.Zero),
            expectedOutputCell: "2023-06-23T11:12:13.4567890+00:00");
    }

    [Fact]
    public async Task TestTimestampTz_When_NoFractions()
    {
        await TestTimestampTzRoundTrip(GetName(), "2023-06-23T11:12:13Z",
            new DateTimeOffset(2023, 6, 23, 11, 12, 13, TimeSpan.Zero),
            expectedOutputCell: "2023-06-23T11:12:13.0000000+00:00");
    }

    [Fact]
    public async Task TestTimestampTz_When_MinValue()
    {
        await TestTimestampTzRoundTrip(GetName(), "0001-01-01T00:00:00.0000000Z",
            DateTimeOffset.MinValue,
            expectedOutputCell: "0001-01-01T00:00:00.0000000+00:00");
    }

    [Fact]
    public async Task TestTimestampTz_When_Null()
    {
        await TestColumnRoundTrip<DateTimeOffset?>(
            GetName(),
            new PostgresTimestampTz(1, ColName, true),
            null,
            null);
    }

    private async Task TestTimestampTzRoundTrip(
        string tableName,
        string inputCell,
        DateTimeOffset expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresTimestampTz(1, ColName, true),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
