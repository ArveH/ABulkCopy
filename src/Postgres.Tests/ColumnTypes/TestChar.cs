namespace Postgres.Tests.ColumnTypes;

[Collection(nameof(DatabaseCollection))]
public class TestChar : PgColumnTypeTestBase
{
    public TestChar(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestChar_When_BasicValue()
    {
        await TestCharRoundTrip(GetName(), 10, "Arve");
    }

    [Fact]
    public async Task TestChar_When_ValueIsExactLength()
    {
        await TestCharRoundTrip(GetName(), 10, "1234567890");
    }

    [Fact]
    public async Task TestChar_When_Length1()
    {
        await TestCharRoundTrip(GetName(), 1, "A");
    }

    [Fact]
    public async Task TestChar_When_Null()
    {
        await TestCharRoundTrip(GetName(), 10, null);
    }

    [Fact]
    public async Task TestChar_When_EmptyString()
    {
        await TestCharRoundTrip(GetName(), 10, "");
    }

    [Fact]
    public async Task TestChar_When_TrailingSpaces_Then_SpacesAreLost()
    {
        // char(n) pads with spaces, and trailing spaces are trimmed on copy out,
        // so "Arve  " is indistinguishable from "Arve" after the round trip
        await TestCharRoundTrip(GetName(), 10, "Arve  ",
            expectedFileValue: "Arve");
    }

    [Fact]
    public async Task TestChar_When_LeadingSpaces_Then_SpacesAreKept()
    {
        await TestCharRoundTrip(GetName(), 10, "  Arve");
    }

    [Fact]
    public async Task TestChar_When_SingleQuote()
    {
        await TestCharRoundTrip(GetName(), 15, $"Arve{Constants.QuoteChar}s vodka");
    }

    [Fact]
    public async Task TestChar_When_QuoteAtEnd()
    {
        await TestCharRoundTrip(GetName(), 10, $"123456789{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestChar_When_OnlyQuotes()
    {
        await TestCharRoundTrip(GetName(), 10, $"{Constants.QuoteChar}{Constants.QuoteChar}");
    }

    [Fact]
    public async Task TestChar_When_DoubleQuote()
    {
        await TestCharRoundTrip(GetName(), 10, "1234567\"89");
    }

    [Fact]
    public async Task TestChar_When_Comma()
    {
        await TestCharRoundTrip(GetName(), 10, "One, Two");
    }

    [Fact]
    public async Task TestChar_When_BackslashAtEnd()
    {
        await TestCharRoundTrip(GetName(), 10, "123456789\\");
    }

    [Fact]
    public async Task TestChar_When_Unicode()
    {
        await TestCharRoundTrip(GetName(), 10, "AﯵChar");
    }

    [Fact]
    public async Task TestChar_When_MultipleRows()
    {
        var tableName = GetName();
        SchemaTableTuple st = (DatabaseFixture.DefaultSchemaName, tableName, false);
        var col = new PostgresChar(1, ColName, true, 10);
        var values = new List<string?> { "Arve", null, "O'Hara", "" };
        var inputRows = values
            .Select(v => ToRow(v == null ? null : QuoteCell(v)))
            .ToList();
        var tableDefinition = await CreateTableAndDataFileAsync(st, col, inputRows);
        try
        {
            // Act (copy in)
            var rowsRead = await CopyInAsync(tableDefinition);

            // Assert
            rowsRead.Should().Be(values.Count);
            (await DbFixture.GetRowCount(st)).Should().Be(values.Count);
            var dbValues = await DbFixture.SelectColumn<string>(st, ColName);
            dbValues.Should().Equal(
                "Arve".PadRight(10),
                null,
                "O'Hara".PadRight(10),
                "".PadRight(10));

            // Act (copy out)
            var outText = await CopyOutAsync(tableDefinition);

            // Assert
            outText.TrimEnd('\r', '\n').Should().Be(
                string.Join(Environment.NewLine, inputRows));
        }
        finally
        {
            await DbFixture.DropTable(st);
        }
    }

    private async Task TestCharRoundTrip(
        string tableName,
        int length,
        string? inputValue,
        string? expectedFileValue = null)
    {
        // char(n) is padded with spaces in the db, and the padding is
        // trimmed again on copy out
        expectedFileValue ??= inputValue?.TrimEnd(' ');
        await TestColumnRoundTrip(
            tableName,
            new PostgresChar(1, ColName, true, length),
            inputValue == null ? null : QuoteCell(inputValue),
            inputValue?.PadRight(length),
            expectedFileValue == null ? null : QuoteCell(expectedFileValue));
    }
}
