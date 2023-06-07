namespace Common.Test.DataFileReader;

public class DataFileReaderTests : CommonTestBase
{
    private const string ColName = "Col1";
    private const string Col2Name = "Col2";
    private static string TestTableName => nameof(DataFileReaderTests);

    private readonly TableDefinition _tableDefinition;
    private readonly FileHelper _fileHelper;
    private readonly IDataFileReaderFactory _dfrFactory;

    public DataFileReaderTests(ITestOutputHelper output)
        : base(output)
    {
        _tableDefinition = MssTestData.GetEmpty(TestTableName);
        _fileHelper = new FileHelper(TestTableName, DbServer.SqlServer);
        _dfrFactory = new DataFileReaderFactory(_fileHelper.FileSystem, TestLogger);
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
        var dataFileReader = Arrange($"\"{testValue}\",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    [Fact]
    public void TestReadOneRow_When_VarcharContainingQuote()
    {
        var testValue = "Some \"Value\"";
        // Arrange
        _tableDefinition.Columns.Add(new PostgresVarChar(1, ColName, false, 100, "en_ci_ai"));
        var dataFileReader = Arrange($"\"{testValue.Replace("\"", "\"\"")}\",");

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
        var dataFileReader = Arrange($"\"{testValue}\",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    private IDataFileReader Arrange(string row1, string? row2=null)
    {
        _fileHelper.CreateDataFile(row1, row2);
        var dataFileReader = _dfrFactory.Create(
            _fileHelper.DataFolder, _tableDefinition);
        return dataFileReader;
    }
}