namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestJson : PgColumnTypeTestBase
{
    public TestJson(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestJson_When_Object()
    {
        await TestJsonRoundTrip(GetName(), """{"name": "Arve", "age": 42}""");
    }

    [Fact]
    public async Task TestJson_When_Formatting_Then_ExactTextIsKept()
    {
        // json (unlike jsonb) stores the exact text, so key order and
        // whitespace survive the round trip
        await TestJsonRoundTrip(GetName(), """{"b":1,  "a":2}""");
    }

    [Fact]
    public async Task TestJson_When_Array()
    {
        await TestJsonRoundTrip(GetName(), "[1, 2, 3]");
    }

    [Fact]
    public async Task TestJson_When_Nested()
    {
        await TestJsonRoundTrip(GetName(),
            """{"a": {"b": [1, 2, {"c": "d"}]}}""");
    }

    [Fact]
    public async Task TestJson_When_SingleQuoteInValue()
    {
        await TestJsonRoundTrip(GetName(), """{"name": "O'Hara"}""");
    }

    [Fact]
    public async Task TestJson_When_Unicode()
    {
        await TestJsonRoundTrip(GetName(), """{"name": "Aﯵ"}""");
    }

    [Fact]
    public async Task TestJson_When_EmptyObject()
    {
        await TestJsonRoundTrip(GetName(), "{}");
    }

    [Fact]
    public async Task TestJson_When_Null()
    {
        await TestJsonRoundTrip(GetName(), null);
    }

    private async Task TestJsonRoundTrip(
        string tableName,
        string? inputJson)
    {
        // json stores the exact text, so the db value and the copy out
        // file are identical to the input
        await TestColumnRoundTrip(
            tableName,
            new PostgresJson(1, ColName, true),
            inputJson == null ? null : QuoteCell(inputJson),
            inputJson);
    }
}
