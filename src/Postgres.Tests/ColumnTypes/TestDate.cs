namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestDate : PgColumnTypeTestBase
{
    public TestDate(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestDate_When_BasicValue()
    {
        await TestDateRoundTrip(GetName(), "2023-06-14", new DateOnly(2023, 6, 14));
    }

    [Fact]
    public async Task TestDate_When_MinValue()
    {
        await TestDateRoundTrip(GetName(), "0001-01-01", DateOnly.MinValue);
    }

    [Fact]
    public async Task TestDate_When_MaxValue()
    {
        await TestDateRoundTrip(GetName(), "9999-12-31", DateOnly.MaxValue);
    }

    [Fact]
    public async Task TestDate_When_LeapDay()
    {
        await TestDateRoundTrip(GetName(), "2024-02-29", new DateOnly(2024, 2, 29));
    }

    [Fact]
    public async Task TestDate_When_Null()
    {
        await TestColumnRoundTrip<DateOnly?>(
            GetName(),
            new PostgresDate(1, ColName, true),
            null,
            null);
    }

    private async Task TestDateRoundTrip(
        string tableName,
        string inputCell,
        DateOnly expectedDbValue)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresDate(1, ColName, true),
            inputCell,
            expectedDbValue);
    }
}
