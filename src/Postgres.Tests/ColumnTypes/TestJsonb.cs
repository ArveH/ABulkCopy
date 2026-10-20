namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestJsonb : PgColumnTypeTestBase
{
    public TestJsonb(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestJsonb_When_NormalizedObject()
    {
        // already in Postgres' normalized form, so it round-trips unchanged
        await TestJsonbRoundTrip(GetName(), """{"a": 1, "b": 2}""");
    }

    [Fact]
    public async Task TestJsonb_When_KeysNotSorted_Then_Normalized()
    {
        await TestJsonbRoundTrip(GetName(),
            """{"b": 2, "a": 1}""",
            expectedJson: """{"a": 1, "b": 2}""");
    }

    [Fact]
    public async Task TestJsonb_When_NoWhitespace_Then_Normalized()
    {
        await TestJsonbRoundTrip(GetName(),
            """{"a":1}""",
            expectedJson: """{"a": 1}""");
    }

    [Fact]
    public async Task TestJsonb_When_DuplicateKeys_Then_LastValueWins()
    {
        await TestJsonbRoundTrip(GetName(),
            """{"a": 1, "a": 2}""",
            expectedJson: """{"a": 2}""");
    }

    [Fact]
    public async Task TestJsonb_When_Array()
    {
        await TestJsonbRoundTrip(GetName(), "[1, 2, 3]");
    }

    [Fact]
    public async Task TestJsonb_When_SingleQuoteInValue()
    {
        await TestJsonbRoundTrip(GetName(), """{"name": "O'Hara"}""");
    }

    [Fact]
    public async Task TestJsonb_When_EmptyObject()
    {
        await TestJsonbRoundTrip(GetName(), "{}");
    }

    [Fact]
    public async Task TestJsonb_When_Null()
    {
        await TestJsonbRoundTrip(GetName(), null);
    }

    private async Task TestJsonbRoundTrip(
        string tableName,
        string? inputJson,
        string? expectedJson = null)
    {
        // jsonb is stored in a normalized form (keys sorted, whitespace
        // normalized, duplicate keys removed), so both the db value and
        // the copy out file contain the normalized text
        expectedJson ??= inputJson;
        await TestColumnRoundTrip(
            tableName,
            new PostgresJsonb(1, ColName, true),
            inputJson == null ? null : QuoteCell(inputJson),
            expectedJson,
            expectedJson == null ? null : QuoteCell(expectedJson));
    }
}
