namespace APostgres.Test.DataFileReader;

public class MssDataFileReaderTests : MssDataFileReaderTestBase
{
    private const string ColName = "Col1";
    private static string TestTableName => nameof(MssDataFileReaderTests);

    private readonly TableDefinition _tableDefinition;
    private readonly FileHelper _fileHelper;
    private readonly IDataFileReaderFactory _dfrFactory;

    public MssDataFileReaderTests(ITestOutputHelper output)
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
    public async Task TestReadColumn_When_BigInt()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(_tableDefinition);
        _fileHelper.CreateDataFile(AllTypes.SampleValues.BigInt + ",");
        var dataFileReader = _dfrFactory.Create(
            _fileHelper.DataFolder, _tableDefinition);

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().Be(AllTypes.SampleValues.BigInt.ToString());
    }

    [Fact]
    public async Task TestReadColumn_When_BigInt_And_Null()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(_tableDefinition);
        _fileHelper.CreateDataFile(",");
        var dataFileReader = _dfrFactory.Create(
            _fileHelper.DataFolder, _tableDefinition);

        // Act
        var stringVal = dataFileReader.ReadColumn(ColName);

        // Assert
        stringVal.Should().BeNull();
    }

    [Fact]
    public async Task TestReadColumn_When_TwoRows()
    {
        // Arrange
        _tableDefinition.Columns.Add(new PostgresBigInt(1, ColName, false));
        await PgDbHelper.Instance.DropTable(TestTableName);
        await PgDbHelper.Instance.CreateTable(_tableDefinition);
        _fileHelper.CreateDataFile("1001,", "1002,");
        var dataFileReader = _dfrFactory.Create(
            _fileHelper.DataFolder, _tableDefinition);

        // Act
        var row1Val = dataFileReader.ReadColumn(ColName);
        dataFileReader.ReadNewLine();
        var row2Val = dataFileReader.ReadColumn(ColName);

        // Assert
        row1Val.Should().Be("1001");
        row2Val.Should().Be("1002");
    }

}