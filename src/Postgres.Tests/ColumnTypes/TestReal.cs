namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestReal : PgColumnTypeTestBase
{
    public TestReal(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestReal_When_BasicValue()
    {
        await TestRealRoundTrip(GetName(), "123.12", 123.12f);
    }

    [Fact]
    public async Task TestReal_When_Negative()
    {
        await TestRealRoundTrip(GetName(), "-123.12", -123.12f);
    }

    [Fact]
    public async Task TestReal_When_Zero()
    {
        await TestRealRoundTrip(GetName(), "0", 0f);
    }

    [Fact]
    public async Task TestReal_When_Fraction()
    {
        await TestRealRoundTrip(GetName(), "0.5", 0.5f);
    }

    [Fact]
    public async Task TestReal_When_Null()
    {
        await TestColumnRoundTrip<float?>(
            GetName(),
            new PostgresReal(1, ColName, true),
            null,
            null);
    }

    private async Task TestRealRoundTrip(
        string tableName,
        string inputCell,
        float expectedDbValue,
        string? expectedOutputCell = null)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresReal(1, ColName, true),
            inputCell,
            expectedDbValue,
            expectedOutputCell);
    }
}
