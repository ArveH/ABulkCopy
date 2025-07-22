namespace Common.Tests;

public class TestScriptReader
{
    private const string TestFileName = "TestScriptReader.sql";
    private readonly FileHelper _fileHelper = new();
    private readonly ScriptReader _scriptsReader;

    public TestScriptReader()
    {
        _scriptsReader = new ScriptReader(_fileHelper.FileSystem);
    }
    
    [Fact]
    public async Task TestRead_When_ScriptFileNotFound()
    {
        var act = async () => await _scriptsReader.ReadAsync(TestFileName, CancellationToken.None).ToListAsync();
        
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("File TestScriptReader.sql not found");
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("Statement1", 1)]
    [InlineData("Statement1\n\nStatement2\n", 2)]
    [InlineData("Statement1\nMoreStatement12", 1)]
    public async Task TestRead(string fileContent, int expectedNoOfStatements)
    {
        var fileData = new MockFileData(fileContent, Encoding.UTF8);
        _fileHelper.FileSystem.AddFile(TestFileName, fileData);
        
        var sqlStatements = await _scriptsReader.ReadAsync(TestFileName, CancellationToken.None).ToListAsync();

        sqlStatements.Count.Should().Be(expectedNoOfStatements);
    }
}