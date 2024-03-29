namespace SqlServer.DbTest;

public class MssDataWriterTestsForStrings : MssDataWriterTestBase
{
    public MssDataWriterTestsForStrings(ITestOutputHelper output)
        : base(output, Environment.MachineName + "MssDataWriterTestsForStrings")
    {
    }

    [Fact]
    public async Task TestWriteChar()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 10),
                "Arve",
                "Arve      ".Quote());
    }

    [Fact]
    public async Task TestWriteChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 15),
            $"Arve{Constants.QuoteChar}s vodka",
            $"Arve{Constants.QuoteChar}{Constants.QuoteChar}s vodka   ".Quote());
    }

    [Fact]
    public async Task TestWriteChar_When_BackSlashLastCharacter()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 10),
            "123456789\\",
            "123456789\\".Quote());
    }

    [Fact]
    public async Task TestWriteVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, 15),
            $"Arve{Constants.QuoteChar}s vodka",
            $"Arve{Constants.QuoteChar}{Constants.QuoteChar}s vodka".Quote());
    }

    [Fact]
    public async Task TestWriteVarChar_When_BackSlash()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, 10),
            "\\Arve",
            "\\Arve".Quote());
    }

    [Fact]
    public async Task TestWriteVarCharMax()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, -1),
            String10K,
            String10K.Quote());
    }

    [Fact]
    public async Task TestWriteNChar()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "Arveﯵ",
            "Arveﯵ     ".Quote());
    }

    [Fact]
    public async Task TestWriteNChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 15),
            $"Arveﯵ{Constants.QuoteChar}s vodka",
            $"Arveﯵ{Constants.QuoteChar}{Constants.QuoteChar}s vodka  ".Quote());
    }

    [Fact]
    public async Task TestWriteNChar_When_BackSlashLastCharacter()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "12345678ﯵ\\",
            "12345678ﯵ\\".Quote());
    }

    [Fact]
    public async Task TestWriteNVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, 15),
            $"Arveﯵ{Constants.QuoteChar}s vodka",
            $"Arveﯵ{Constants.QuoteChar}{Constants.QuoteChar}s vodka".Quote());
    }

    [Fact]
    public async Task TestWriteNVarChar_When_BackSlash()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, 10),
            "\\ﯵArve",
            "\\ﯵArve".Quote());
    }

    [Fact]
    public async Task TestWriteNVarCharMax()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, -1),
            NString10K,
            NString10K.Quote());
    }

    [Fact]
    public async Task TestWriteText()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerText(101, "MyTestCol", false), 
            String10K,
            SqlDbType.Text);

        jsonTxt.TrimEnd().Should().Be($"{String10K.Quote()}{Constants.ColumnSeparatorChar}");
    }

    [Fact]
    public async Task TestWriteNText()
    {
        var jsonTxt = await ArrangeAndAct(
            new SqlServerNText(101, "MyTestCol", false), 
            NString10K,
            SqlDbType.NText);

        jsonTxt.TrimEnd().Should().Be($"{NString10K.Quote()}{Constants.ColumnSeparatorChar}");
    }
}