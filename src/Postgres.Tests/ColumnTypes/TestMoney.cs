namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestMoney : PgColumnTypeTestBase
{
    public TestMoney(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestMoney_When_BasicValue()
    {
        await TestMoneyRoundTrip(GetName(), "123.12", 123.12m);
    }

    [Fact]
    public async Task TestMoney_When_Negative()
    {
        await TestMoneyRoundTrip(GetName(), "-123.12", -123.12m);
    }

    [Fact]
    public async Task TestMoney_When_Zero()
    {
        await TestMoneyRoundTrip(GetName(), "0", 0m);
    }

    [Fact]
    public async Task TestMoney_When_MaxValue()
    {
        await TestMoneyRoundTrip(GetName(),
            "92233720368547758.07", 92233720368547758.07m);
    }

    [Fact]
    public async Task TestMoney_When_MinValue()
    {
        await TestMoneyRoundTrip(GetName(),
            "-92233720368547758.08", -92233720368547758.08m);
    }

    [Fact]
    public async Task TestMoney_When_Null()
    {
        await TestColumnRoundTrip<decimal?>(
            GetName(),
            new PostgresMoney(1, ColName, true),
            null,
            null);
    }

    private async Task TestMoneyRoundTrip(
        string tableName,
        string inputCell,
        decimal expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresMoney(1, ColName, true),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
