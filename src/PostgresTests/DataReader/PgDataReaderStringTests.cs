namespace Postgres.Tests.DataReader;

[Collection(nameof(DatabaseCollection))]
public class PgDataReaderStringTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderStringTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestChar()
    {
        // Arrange
        var testVal = "AﯵChar";
        var col = new PostgresChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal.PadRight(10, ' '));
    }

    [Fact]
    public async Task TestChar_When_Null()
    {
        // Arrange
        var col = new PostgresChar(1, ColName, true, 10);
        var colValue = await TestDataReader<string?>(
            GetName(), col, Constants.ColumnSeparator);

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestVarChar()
    {
        // Arrange
        var testVal = "Some value";
        var col = new PostgresVarChar(1, ColName, false, 100);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_QuoteAtEnd()
    {
        // Arrange
        var testVal = $"123456789{Constants.QuoteChar}";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal.Replace(Constants.Quote, Constants.Quote+Constants.Quote)}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_2Quotes()
    {
        // Arrange
        var testVal = $"12345678{Constants.QuoteChar}{Constants.QuoteChar}";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal.Replace(Constants.Quote, Constants.Quote+Constants.Quote)}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_SingleQuote()
    {
        // Arrange
        var testVal = "1234567'89";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal.Replace(Constants.Quote, Constants.Quote + Constants.Quote)}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_DoubleQuote()
    {
        // Arrange
        var testVal = "1234567\"89";
        var col = new PostgresVarChar(1, ColName, false, 10);
        var colValue = await TestDataReader<string>(
            GetName(), col, $"{Constants.QuoteChar}{testVal.Replace(Constants.Quote, Constants.Quote + Constants.Quote)}{Constants.QuoteChar}{Constants.ColumnSeparatorChar}");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestVarChar_When_Null()
    {
        // Arrange
        var col = new PostgresVarChar(1, ColName, true, 10);
        var colValue = await TestDataReader<string?>(
            GetName(), col, Constants.ColumnSeparator);

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestVarChar_When_Comma()
    {
        // Arrange
        var col = new PostgresVarChar(1, ColName, true, 50);
        var colValue = await TestDataReader<string?>(
            GetName(), col, $"{Constants.QuoteChar}One, Two{Constants.QuoteChar},");

        colValue.Should().Be("One, Two");
    }

    [Fact]
    public async Task TestVarChar_When_EmptyString()
    {
        // Arrange
        var col = new PostgresVarChar(1, ColName, true, 50);
        var colValue = await TestDataReader<string?>(
            GetName(), col, $"{Constants.QuoteChar}{Constants.QuoteChar},");

        colValue.Should().Be("");
    }

    [Fact]
    public async Task TestVarChar_When_EmptyStringAndEmptyStringFlag()
    {
        // Arrange
        var testVal = $"{Constants.QuoteChar}{Constants.QuoteChar},";
        var expectedVal = " ";
        var tableName = GetName();
        var col = new PostgresVarChar(1, ColName, false, 10);
        var tableDefinition = await CreateTableAndDataFile(
            tableName, 
            new() { col }, 
            new() {testVal});
        var dataReader = new PgDataReader(
            DbFixture.PgContext,
            QBFactoryMock.Object,
            new DataFileReader(FileHelper.FileSystem, TestLogger),
            TestLogger);
        var cts = new CancellationTokenSource();

        // Act
        await dataReader.ReadAsync(FileHelper.DataFolder, tableDefinition, cts.Token, EmptyStringFlag.Single);

        //Assert
        var colValue = await DbFixture.SelectScalar<string>(
            tableName, col);
        colValue.Should().Be(expectedVal);
    }
}