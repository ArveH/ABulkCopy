namespace ASqlServer.Test;

public class MssDataWriterTestsForStrings : MssDataWriterTestBase
{
    public override string _testTableName => Environment.MachineName + "MssDataWriterTestsForStrings";

    public MssDataWriterTestsForStrings(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestWriteChar()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 10),
                "Arve",
                "'Arve      '");
    }

    [Fact]
    public async Task TestWriteChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 15),
            "Arve's vodka",
            "'Arve''s vodka   '");
    }

    [Fact]
    public async Task TestWriteChar_When_BackSlashLastCharacter()
    {
        await TestWrite(
            new SqlServerChar(101, "MyTestCol", false, 10),
            "123456789\\",
            "'123456789\\'");
    }

    [Fact]
    public async Task TestWriteVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, 15),
            "Arve's vodka",
            "'Arve''s vodka'");
    }

    [Fact]
    public async Task TestWriteVarChar_When_BackSlash()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, 10),
            "\\Arve",
            "'\\Arve'");
    }

    [Fact]
    public async Task TestWriteVarCharMax()
    {
        await TestWrite(
            new SqlServerVarChar(101, "MyTestCol", false, -1),
            String10K,
            "'" + String10K + "'");
    }

    [Fact]
    public async Task TestWriteNChar()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "Arveﯵ",
            "'Arveﯵ'");
    }

    [Fact]
    public async Task TestWriteNChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 15),
            "Arveﯵ's vodka",
            "'Arveﯵ''s vodka'");
    }

    [Fact]
    public async Task TestWriteNChar_When_BackSlashLastCharacter()
    {
        await TestWrite(
            new SqlServerNChar(101, "MyTestCol", false, 10),
            "12345678ﯵ\\",
            "'12345678ﯵ\\'");
    }

    [Fact]
    public async Task TestWriteNVarChar_When_SingleQuote_Then_TwoQuotesInFile()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, 15),
            "Arveﯵ's vodka",
            "'Arveﯵ''s vodka'");
    }

    [Fact]
    public async Task TestWriteNVarChar_When_BackSlash()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, 10),
            "\\ﯵArve",
            "'\\ﯵArve'");
    }

    [Fact]
    public async Task TestWriteNVarCharMax()
    {
        await TestWrite(
            new SqlServerNVarChar(101, "MyTestCol", false, -1),
            NString10K,
            "'" + NString10K + "'");
    }
}