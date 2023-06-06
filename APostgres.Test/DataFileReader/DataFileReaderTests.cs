namespace APostgres.Test.DataFileReader;

public class DataFileReaderTests : PgTestBase
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
        _fileHelper = new FileHelper(TestTableName, DbServer.SqlServer)
        {
            DataFolder = @"C:\testdata"
        };
        _dfrFactory = new DataFileReaderFactory(_fileHelper.FileSystem, TestLogger);
    }

    [Fact]
    public async Task TestReadOneRow_When_BigInt()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = await Arrange(AllTypes.SampleValues.BigInt + ",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(AllTypes.SampleValues.BigInt.ToString());
    }

    [Fact]
    public async Task TestReadOneRow_When_2BigInts()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        _tableDefinition.Columns.Add(new PostgresBigInt(2, Col2Name, false));
        var dataFileReader = await Arrange("1001,1002,");

        // Act
        var col1Val = dataFileReader.ReadColumn(ColName);
        var col2Val = dataFileReader.ReadColumn(ColName);

        // Assert
        col1Val.Should().Be("1001");
        col2Val.Should().Be("1002");
    }

    [Fact]
    public async Task TestReadOneRow_When_BigInt_And_Null()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = await Arrange(",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().BeNull();
    }

    [Fact]
    public async Task TestReadTwoRows_When_BigInt()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        var dataFileReader = await Arrange("1001,", "1002,");

        // Act
        var row1Val = dataFileReader.ReadColumn(ColName);
        dataFileReader.ReadNewLine();
        var row2Val = dataFileReader.ReadColumn(Col2Name);

        // Assert
        row1Val.Should().Be("1001");
        row2Val.Should().Be("1002");
    }

    [Fact]
    public async Task TestReadOneRow_When_Varchar()
    {
        var testValue = "Some Value";
        // Arrange
        _tableDefinition.Columns.Add(new PostgresVarChar(1, ColName, false, 100, "en_ci_ai"));
        var dataFileReader = await Arrange($"\"{testValue}\",");

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(testValue);
    }

    private async Task<IDataFileReader> Arrange(string row1, string? row2=null)
    {
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(_tableDefinition);
        _fileHelper.CreateDataFile(row1, row2);
        var dataFileReader = _dfrFactory.Create(
            _fileHelper.DataFolder, _tableDefinition);
        return dataFileReader;
    }
}