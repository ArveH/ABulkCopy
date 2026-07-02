namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestTimestamp : PgColumnTypeTestBase
{
    public TestTimestamp(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestTimestamp_When_NoFractions()
    {
        // copy out always writes the round-trip ("O") format
        await TestTimestampRoundTrip(GetName(), "2023-06-23T11:12:13",
            new DateTime(2023, 6, 23, 11, 12, 13),
            expectedOutputCell: "2023-06-23T11:12:13.0000000");
    }

    [Fact]
    public async Task TestTimestamp_When_Fractions()
    {
        await TestTimestampRoundTrip(GetName(), "2023-06-23T11:12:13.456789Z",
            new DateTime(2023, 6, 23, 11, 12, 13, 456, 789),
            expectedOutputCell: "2023-06-23T11:12:13.4567890");
    }

    [Fact]
    public async Task TestTimestamp_When_MinValue()
    {
        await TestTimestampRoundTrip(GetName(), "0001-01-01T00:00:00.0000000Z",
            DateTime.MinValue,
            expectedOutputCell: "0001-01-01T00:00:00.0000000");
    }

    [Fact]
    public async Task TestTimestamp_When_Null()
    {
        await TestColumnRoundTrip<DateTime?>(
            GetName(),
            new PostgresTimestamp(1, ColName, true),
            null,
            null);
    }

    private async Task TestTimestampRoundTrip(
        string tableName,
        string inputCell,
        DateTime expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresTimestamp(1, ColName, true),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
