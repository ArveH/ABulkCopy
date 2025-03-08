namespace Common.Tests;

public class DataFileReaderTests : CommonTestBase
{
    private const string ColName = "Col1";
    private const string Col2Name = "Col2";
    private static string TestTableName => nameof(DataFileReaderTests);

    private readonly TableDefinition _tableDefinition;
    private readonly FileHelper _fileHelper;

    public DataFileReaderTests(ITestOutputHelper output)
        : base(output)
    {
        _tableDefinition = MssTestData.GetEmpty(("dbo", TestTableName));
        _fileHelper = new FileHelper();
    }

    [Fact]
    public void TestReadOneRow_When_BigInt()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = Arrange(AllTypes.SampleValues.BigInt + ",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(AllTypes.SampleValues.BigInt.ToString());
    }

    [Fact]
    public void TestReadOneRow_When_2BigInts()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        _tableDefinition.Columns.Add(new PostgresBigInt(2, Col2Name, false));
        var dataFileReader = Arrange("1001,1002,");

        // Act
        var col1Val = dataFileReader.ReadColumn(ColName);
        var col2Val = dataFileReader.ReadColumn(ColName);

        // Assert
        col1Val.Should().Be("1001");
        col2Val.Should().Be("1002");
    }

    [Fact]
    public void TestReadOneRow_When_BigInt_And_Null()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = Arrange(",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().BeNull();
    }

    [Fact]
    public void TestReadTwoRows_When_BigInt()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = Arrange("1001,", "1002,");

        // Act
        var row1Val = dataFileReader.ReadColumn(ColName);
        dataFileReader.ReadNewLine();
        var row2Val = dataFileReader.ReadColumn(Col2Name);

        // Assert
        row1Val.Should().Be("1001");
        row2Val.Should().Be("1002");
    }

    [Fact]
    public void TestReadOneRow_When_Varchar()
    {
        var testValue = "Some Value";
        // Arrange
        _tableDefinition.Columns.Add(new PostgresVarChar(1, ColName, false, 100, "en_ci_ai"));
        var dataFileReader = Arrange($"{Constants.QuoteChar}{testValue}{Constants.QuoteChar},");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    [Fact]
    public void TestReadOneRow_When_VarcharContainingQuote()
    {
        var testValue = $"Some {Constants.QuoteChar}Value{Constants.QuoteChar}";
        // Arrange
        _tableDefinition.Columns.Add(new PostgresVarChar(1, ColName, false, 100, "en_ci_ai"));
        var dataFileReader = Arrange($"{Constants.QuoteChar}{testValue.Replace(Constants.Quote, Constants.Quote+Constants.Quote)}{Constants.QuoteChar},");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    [Fact]
    public void TestReadOneRow_When_CharWithUnicode()
    {
        var testValue = "AﯵChar";
        // Arrange
        _tableDefinition.Columns.Add(new PostgresChar(1, ColName, false, 10, "en_ci_ai"));
        var dataFileReader = Arrange($"{Constants.QuoteChar}{testValue}{Constants.QuoteChar},");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    [Theory]
    [InlineData("Test value", EmptyStringFlag.Leave, "Test value")]
    [InlineData("Test value", EmptyStringFlag.Single, "Test value")]
    [InlineData("Test value", EmptyStringFlag.Empty, "Test value")]
    [InlineData("Test value", EmptyStringFlag.ForceSingle, "Test value")]
    [InlineData("Test value", EmptyStringFlag.ForceEmpty, "Test value")]
    [InlineData("", EmptyStringFlag.Leave, "")]
    [InlineData("", EmptyStringFlag.Single, " ")]
    [InlineData("", EmptyStringFlag.Empty, "")]
    [InlineData("", EmptyStringFlag.ForceSingle, " ")]
    [InlineData("", EmptyStringFlag.ForceEmpty, "")]
    [InlineData(" ", EmptyStringFlag.Leave, " ")]
    [InlineData(" ", EmptyStringFlag.Single, " ")]
    [InlineData(" ", EmptyStringFlag.Empty, "")]
    [InlineData(" ", EmptyStringFlag.ForceSingle, " ")]
    [InlineData(" ", EmptyStringFlag.ForceEmpty, "")]
    [InlineData("  ", EmptyStringFlag.Leave, "  ")]
    [InlineData("  ", EmptyStringFlag.Single, "  ")]
    [InlineData("  ", EmptyStringFlag.Empty, "  ")]
    [InlineData("  ", EmptyStringFlag.ForceSingle, " ")]
    [InlineData("  ", EmptyStringFlag.ForceEmpty, "")]
    public void TestReadOneRow_When_UsingEmptyStringFlag(
        string testValue, EmptyStringFlag emptyStringFlag, string expectedValue)
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresVarChar(1, ColName, false, 100, "en_ci_as"));
        var dataFileReader = Arrange($"{Constants.QuoteChar}{testValue}{Constants.QuoteChar},", null, emptyStringFlag);

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(expectedValue);
    }

    private IDataFileReader Arrange(string row1, string? row2 = null, EmptyStringFlag emptyStringFlag = EmptyStringFlag.Leave)
    {
        var rows = new List<string> { row1 };
        if (row2 != null)
        {
            rows.Add(row2);
        }
        _fileHelper.CreateDataFile(("dbo", TestTableName), rows);
        var dataFileReader = new DataFileReader(_fileHelper.FileSystem, TestLogger);
        var path = Path.Combine(
            _fileHelper.DataFolder,
            $"dbo.{TestTableName}{Constants.DataSuffix}");
        dataFileReader.Open(path, new InsertSettings{EmptyStringFlag = emptyStringFlag});
        return dataFileReader;
    }
}