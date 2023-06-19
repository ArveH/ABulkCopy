namespace APostgres.Test.DataReader;

public class PgDataReaderStringTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderStringTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public async Task TestChar()
    {
        // Arrange
        var testVal = "AﯵChar";
        var col = new PostgresChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"\"{testVal}\",");

        colValue.Should().Be(testVal.PadRight(10, ' '));
    }

    [Fact]
    public async Task TestChar_When_Null()
    {
        // Arrange
        var col = new PostgresChar(1, ColName, true, 10);
        var colValue = await TestDataReader<string?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestVarChar()
    {
        // Arrange
        var testVal = "Some value";
        var col = new PostgresVarChar(1, ColName, false, 100);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"\"{testVal}\",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_QuoteAtEnd()
    {
        // Arrange
        var testVal = "123456789\"";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"\"{testVal.Replace("\"", "\"\"")}\",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_2Quotes()
    {
        // Arrange
        var testVal = "12345678\"\"";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"\"{testVal.Replace("\"", "\"\"")}\",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_SingleQuote()
    {
        // Arrange
        var testVal = "1234567'89";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"\"{testVal.Replace("\"", "\"\"")}\",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_Null()
    {
        // Arrange
        var col = new PostgresVarChar(1, ColName, true, 10);
        var colValue = await TestDataReader<string?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestVarChar_When_Comma()
    {
        // Arrange
        var col = new PostgresVarChar(1, ColName, true, 10);
        var colValue = await TestDataReader<string?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }
}