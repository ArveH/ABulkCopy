namespace Common.Test;

public class DataFileReaderTests
{
    private const string TestPath = @"C:\testfiles";
    private static string TestTableName => nameof(DataFileReaderTests);
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private FileHelper _fileHelper;

    protected DataFileReaderTests(ITestOutputHelper output)
    {
        _originalTableDefinition = MssTestData.GetEmpty(TestTableName);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _fileHelper = new FileHelper(TestTableName, DbServer.Postgres);
    }

    [Fact]
    public async Task Method()
    {
        await Task.CompletedTask;
    }
}