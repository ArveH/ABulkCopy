namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestVarChar : PgColumnTypeTestBase
{
    public TestVarChar(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestVarChar_When_BasicValue()
    {
        await TestVarCharRoundTrip(GetName(), 50, "Some value");
    }

    [Fact]
    public async Task TestVarChar_When_ValueIsExactLength()
    {
        await TestVarCharRoundTrip(GetName(), 10, "1234567890");
    }

    [Fact]
    public async Task TestVarChar_When_Null()
    {
        await TestVarCharRoundTrip(GetName(), 10, null);
    }

    [Fact]
    public async Task TestVarChar_When_EmptyString()
    {
        await TestVarCharRoundTrip(GetName(), 10, "");
    }

    [Fact]
    public async Task TestVarChar_When_TrailingSpaces_Then_TrimmedOnCopyOut()
    {
        // varchar keeps trailing spaces in the db, but the file format
        // trims them on copy out
        await TestVarCharRoundTrip(GetName(), 10, "Arve  ",
            expectedFileValue: "Arve");
    }

    [Fact]
    public async Task TestVarChar_When_LeadingSpaces_Then_SpacesAreKept()
    {
        await TestVarCharRoundTrip(GetName(), 10, "  Arve");
    }

    [Fact]
    public async Task TestVarChar_When_SingleQuote()
    {
        await TestVarCharRoundTrip(GetName(), 15, $"Arve{Constants.QuoteChar}s vodka");
    }

    [Fact]
    public async Task TestVarChar_When_QuoteAtEnd()
    {
        await TestVarCharRoundTrip(GetName(), 10, $"123456789{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestVarChar_When_OnlyQuotes()
    {
        await TestVarCharRoundTrip(GetName(), 10, $"{Constants.QuoteChar}{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestVarChar_When_DoubleQuote()
    {
        await TestVarCharRoundTrip(GetName(), 10, "1234567\"89");
    }

    [Fact]
    public async Task TestVarChar_When_Comma()
    {
        await TestVarCharRoundTrip(GetName(), 10, "One, Two");
    }

    [Fact]
    public async Task TestVarChar_When_BackslashAtEnd()
    {
        await TestVarCharRoundTrip(GetName(), 10, "123456789\\");
    }

    [Fact]
    public async Task TestVarChar_When_Unicode()
    {
        await TestVarCharRoundTrip(GetName(), 20, "AﯵVarChar");
    }

    [Fact]
    public async Task TestVarChar_When_NoLength_Then_TextIsUsed()
    {
        // length < 1 turns the column into text, which allows a 10K value
        await TestVarCharRoundTrip(GetName(), -1, AllTypes.SampleValues.Varchar10K);
    }

    private async Task TestVarCharRoundTrip(
        string tableName,
        int length,
        string? inputValue,
        string? expectedFileValue = null)
    {
        // trailing spaces are kept in the db but trimmed on copy out
        expectedFileValue ??= inputValue?.TrimEnd(' ');
        await TestColumnRoundTrip(
            tableName,
            new PostgresVarChar(1, ColName, true, length),
            inputValue == null ? null : QuoteCell(inputValue),
            inputValue,
            expectedFileValue == null ? null : QuoteCell(expectedFileValue));
    }
}
