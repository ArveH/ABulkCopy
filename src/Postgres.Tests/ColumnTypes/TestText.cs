namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestText : PgColumnTypeTestBase
{
    public TestText(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestText_When_BasicValue()
    {
        await TestTextRoundTrip(GetName(), "Some text value");
    }

    [Fact]
    public async Task TestText_When_Null()
    {
        await TestTextRoundTrip(GetName(), null);
    }

    [Fact]
    public async Task TestText_When_EmptyString()
    {
        await TestTextRoundTrip(GetName(), "");
    }

    [Fact]
    public async Task TestText_When_SingleQuote()
    {
        await TestTextRoundTrip(GetName(), $"Arve{Constants.QuoteChar}s vodka");
    }

    [Fact]
    public async Task TestText_When_Comma()
    {
        await TestTextRoundTrip(GetName(), "One, Two");
    }

    [Fact]
    public async Task TestText_When_Unicode()
    {
        await TestTextRoundTrip(GetName(), "AﯵText");
    }

    [Fact]
    public async Task TestText_When_Newline()
    {
        await TestTextRoundTrip(GetName(), "Line1\nLine2");
    }

    [Fact]
    public async Task TestText_When_10KValue()
    {
        await TestTextRoundTrip(GetName(), AllTypes.SampleValues.Varchar10K);
    }

    [Fact]
    public async Task TestText_When_10KUnicodeValue()
    {
        await TestTextRoundTrip(GetName(), AllTypes.SampleValues.NVarchar10K);
    }

    [Fact]
    public async Task TestText_When_TrailingSpaces_Then_TrimmedOnCopyOut()
    {
        // text keeps trailing spaces in the db, but the file format
        // trims them on copy out
        await TestTextRoundTrip(GetName(), "Arve  ",
            expectedFileValue: "Arve");
    }

    private async Task TestTextRoundTrip(
        string tableName,
        string? inputValue,
        string? expectedFileValue = null)
    {
        expectedFileValue ??= inputValue?.TrimEnd(' ');
        await TestColumnRoundTrip(
            tableName,
            new PostgresText(1, ColName, true),
            inputValue == null ? null : QuoteCell(inputValue),
            inputValue,
            expectedFileValue == null ? null : QuoteCell(expectedFileValue));
    }
}
