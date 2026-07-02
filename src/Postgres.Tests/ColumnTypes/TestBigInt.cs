namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestBigInt : PgColumnTypeTestBase
{
    public TestBigInt(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestBigInt_When_BasicValue()
    {
        await TestBigIntRoundTrip(GetName(), AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestBigInt_When_Zero()
    {
        await TestBigIntRoundTrip(GetName(), 0L);
    }

    [Fact]
    public async Task TestBigInt_When_Negative()
    {
        await TestBigIntRoundTrip(GetName(), -123456789012345L);
    }

    [Fact]
    public async Task TestBigInt_When_MinValue()
    {
        await TestBigIntRoundTrip(GetName(), long.MinValue);
    }

    [Fact]
    public async Task TestBigInt_When_MaxValue()
    {
        await TestBigIntRoundTrip(GetName(), long.MaxValue);
    }

    [Fact]
    public async Task TestBigInt_When_Null()
    {
        await TestColumnRoundTrip<long?>(
            GetName(),
            new PostgresBigInt(1, ColName, true),
            null,
            null);
    }

    private async Task TestBigIntRoundTrip(string tableName, long value)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresBigInt(1, ColName, true),
            value.ToString(),
            value);
    }
}
