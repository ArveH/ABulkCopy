namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestBoolean : PgColumnTypeTestBase
{
    public TestBoolean(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestBoolean_When_True()
    {
        await TestColumnRoundTrip(
            GetName(),
            new PostgresBoolean(1, ColName, true),
            "true",
            true);
    }

    [Fact]
    public async Task TestBoolean_When_False()
    {
        await TestColumnRoundTrip(
            GetName(),
            new PostgresBoolean(1, ColName, true),
            "false",
            false);
    }

    [Fact]
    public async Task TestBoolean_When_1_Then_WrittenAsTrue()
    {
        // "1" is accepted on copy in, but copy out always writes "true"
        await TestColumnRoundTrip(
            GetName(),
            new PostgresBoolean(1, ColName, true),
            "1",
            true,
            expectedOutputCell: "true");
    }

    [Fact]
    public async Task TestBoolean_When_0_Then_WrittenAsFalse()
    {
        // "0" is accepted on copy in, but copy out always writes "false"
        await TestColumnRoundTrip(
            GetName(),
            new PostgresBoolean(1, ColName, true),
            "0",
            false,
            expectedOutputCell: "false");
    }

    [Fact]
    public async Task TestBoolean_When_Null()
    {
        await TestColumnRoundTrip<bool?>(
            GetName(),
            new PostgresBoolean(1, ColName, true),
            null,
            null);
    }
}
