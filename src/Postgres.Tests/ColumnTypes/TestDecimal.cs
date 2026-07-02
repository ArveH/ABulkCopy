namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestDecimal : PgColumnTypeTestBase
{
    public TestDecimal(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestDecimal_When_SmallValue()
    {
        await TestDecimalRoundTrip(GetName(), 4, 0, "1234", 1234m);
    }

    [Fact]
    public async Task TestDecimal_When_LargeValue()
    {
        await TestDecimalRoundTrip(GetName(), 32, 6,
            "12345678901234567890.123456", 12345678901234567890.123456m);
    }

    [Fact]
    public async Task TestDecimal_When_Negative()
    {
        await TestDecimalRoundTrip(GetName(), 10, 2, "-1234.56", -1234.56m);
    }

    [Fact]
    public async Task TestDecimal_When_Zero()
    {
        await TestDecimalRoundTrip(GetName(), 10, 2, "0", 0m);
    }

    [Fact]
    public async Task TestDecimal_When_TrailingZeros_Then_TrimmedOnCopyOut()
    {
        // the file format trims trailing zeros in the fraction
        await TestDecimalRoundTrip(GetName(), 10, 2, "1234.50", 1234.50m,
            expectedOutputCell: "1234.5");
    }

    [Fact]
    public async Task TestDecimal_When_MoreDecimalsThanScale_Then_Rounded()
    {
        // Postgres rounds the value to the column's scale
        await TestDecimalRoundTrip(GetName(), 10, 2, "1.999", 2m,
            expectedOutputCell: "2");
    }

    [Fact]
    public async Task TestDecimal_When_Null()
    {
        await TestColumnRoundTrip<decimal?>(
            GetName(),
            new PostgresDecimal(1, ColName, true, 20, 2),
            null,
            null);
    }

    private async Task TestDecimalRoundTrip(
        string tableName,
        int precision,
        int scale,
        string inputCell,
        decimal expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresDecimal(1, ColName, true, precision, scale),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
