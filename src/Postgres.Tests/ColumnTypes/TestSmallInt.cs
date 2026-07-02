namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestSmallInt : PgColumnTypeTestBase
{
    public TestSmallInt(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestSmallInt_When_BasicValue()
    {
        await TestSmallIntRoundTrip(GetName(), AllTypes.SampleValues.SmallInt);
    }

    [Fact]
    public async Task TestSmallInt_When_Zero()
    {
        await TestSmallIntRoundTrip(GetName(), 0);
    }

    [Fact]
    public async Task TestSmallInt_When_Negative()
    {
        await TestSmallIntRoundTrip(GetName(), -12345);
    }

    [Fact]
    public async Task TestSmallInt_When_MinValue()
    {
        await TestSmallIntRoundTrip(GetName(), short.MinValue);
    }

    [Fact]
    public async Task TestSmallInt_When_MaxValue()
    {
        await TestSmallIntRoundTrip(GetName(), short.MaxValue);
    }

    [Fact]
    public async Task TestSmallInt_When_Null()
    {
        await TestColumnRoundTrip<int?>(
            GetName(),
            new PostgresSmallInt(1, ColName, true),
            null,
            null);
    }

    private async Task TestSmallIntRoundTrip(string tableName, int value)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresSmallInt(1, ColName, true),
            value.ToString(),
            value);
    }
}
