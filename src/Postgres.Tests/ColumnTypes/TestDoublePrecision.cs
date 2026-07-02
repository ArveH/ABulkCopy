namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestDoublePrecision : PgColumnTypeTestBase
{
    public TestDoublePrecision(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestDoublePrecision_When_BasicValue()
    {
        await TestDoubleRoundTrip(GetName(), "12345678.123456", 12345678.123456d);
    }

    [Fact]
    public async Task TestDoublePrecision_When_Negative()
    {
        await TestDoubleRoundTrip(GetName(), "-12345678.123456", -12345678.123456d);
    }

    [Fact]
    public async Task TestDoublePrecision_When_Zero()
    {
        await TestDoubleRoundTrip(GetName(), "0", 0d);
    }

    [Fact]
    public async Task TestDoublePrecision_When_Fraction()
    {
        await TestDoubleRoundTrip(GetName(), "0.5", 0.5d);
    }

    [Fact]
    public async Task TestDoublePrecision_When_Null()
    {
        await TestColumnRoundTrip<double?>(
            GetName(),
            new PostgresDoublePrecision(1, ColName, true),
            null,
            null);
    }

    private async Task TestDoubleRoundTrip(
        string tableName,
        string inputCell,
        double expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresDoublePrecision(1, ColName, true),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
