namespace ASqlServer.Test;

public abstract class MssDataWriterTestBase : MssTestBase
{
    public const string TestPath = @"C:\testfiles";
    public abstract string _testTableName { get; }
    protected readonly TableDefinition _originalTableDefinition;
    protected readonly MockFileSystem _mockFileSystem;
    protected readonly IDataWriter _dataWriter;

    protected MssDataWriterTestBase(ITestOutputHelper output)
        : base(output)
    {
        _originalTableDefinition = MssTestData.GetEmpty(_testTableName);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _dataWriter = new DataWriter(
            MssDbContext, 
            new TableReaderFactory(new SelectCreator(_logger), _logger),
            _mockFileSystem, _logger);
    }

    protected async Task<string> ArrangeAndAct(IColumn col, object? value, SqlDbType? dbType = null)
    {
        // Arrange
        _originalTableDefinition.Columns.Add(col);
        await MssDbHelper.Instance.DropTable(_testTableName);
        await MssDbHelper.Instance.CreateTable(_originalTableDefinition);
        await MssDbHelper.Instance.InsertIntoSingleColumnTable(
            _testTableName, value, dbType);

        // Act
        try
        {
            await _dataWriter.Write(
                _originalTableDefinition,
                TestPath);
        }
        finally
        {
            await MssDbHelper.Instance.DropTable(_testTableName);
        }

        return await GetJsonText();
    }

    protected async Task TestWriteUsingType(IColumn col, object? value)
    {
        // Assert
        var jsonTxt = await ArrangeAndAct(col, value);
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }

    protected async Task TestWrite(
        IColumn col,
        string actual,
        string? expected = null)
    {
        var jsonTxt = await ArrangeAndAct(col, actual);
        expected ??= actual;
        jsonTxt.TrimEnd().Should().Be($"{expected},");
    }

    protected static readonly string String10K = new ('a', 10000);
    protected static readonly string NString10K = new ('ﯵ', 10000);

    protected async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, _testTableName + CommonConstants.DataSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because data file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}