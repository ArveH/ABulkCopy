namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestTime : PgColumnTypeTestBase
{
    public TestTime(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestTime_When_NoFractions()
    {
        await TestTimeRoundTrip(GetName(), "11:12:13", new TimeOnly(11, 12, 13));
    }

    [Fact]
    public async Task TestTime_When_Fractions()
    {
        // copy out always writes 7 fraction digits (TimeSpan round-trip format)
        await TestTimeRoundTrip(GetName(), "11:12:13.456789",
            new TimeOnly(11, 12, 13, 456, 789),
            expectedOutputCell: "11:12:13.4567890");
    }

    [Fact]
    public async Task TestTime_When_Midnight()
    {
        await TestTimeRoundTrip(GetName(), "00:00:00", TimeOnly.MinValue);
    }

    [Fact]
    public async Task TestTime_When_EndOfDay()
    {
        await TestTimeRoundTrip(GetName(), "23:59:59.999999",
            new TimeOnly(23, 59, 59, 999, 999),
            expectedOutputCell: "23:59:59.9999990");
    }

    [Fact]
    public async Task TestTime_When_LowerPrecision_Then_Rounded()
    {
        // time(2) rounds the fraction to 2 digits
        await TestTimeRoundTrip(GetName(), "11:12:13.456789",
            new TimeOnly(11, 12, 13, 460),
            expectedOutputCell: "11:12:13.4600000",
            precision: 2);
    }

    [Fact]
    public async Task TestTime_When_Null()
    {
        await TestColumnRoundTrip<TimeOnly?>(
            GetName(),
            new PostgresTime(1, ColName, true),
            null,
            null);
    }

    private async Task TestTimeRoundTrip(
        string tableName,
        string inputCell,
        TimeOnly expectedDbValue,
        string? expectedOutputCell = null,
        int precision = 6)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresTime(1, ColName, true, precision),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
