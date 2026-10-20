namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestInt : PgColumnTypeTestBase
{
    public TestInt(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestInt_When_BasicValue()
    {
        await TestIntRoundTrip(GetName(), AllTypes.SampleValues.Int);
    }

    [Fact]
    public async Task TestInt_When_Zero()
    {
        await TestIntRoundTrip(GetName(), 0);
    }

    [Fact]
    public async Task TestInt_When_Negative()
    {
        await TestIntRoundTrip(GetName(), -123456789);
    }

    [Fact]
    public async Task TestInt_When_MinValue()
    {
        await TestIntRoundTrip(GetName(), int.MinValue);
    }

    [Fact]
    public async Task TestInt_When_MaxValue()
    {
        await TestIntRoundTrip(GetName(), int.MaxValue);
    }

    [Fact]
    public async Task TestInt_When_Null()
    {
        await TestColumnRoundTrip<int?>(
            GetName(),
            new PostgresInt(1, ColName, true),
            null,
            null);
    }

    private async Task TestIntRoundTrip(string tableName, int value)
    {
        await TestColumnRoundTrip(
            tableName,
            new PostgresInt(1, ColName, true),
            value.ToString(),
            value);
    }
}
