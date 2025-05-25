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
        var act = async () => await _scriptsReader.ReadAsync(TestFileName).ToListAsync();
        
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("File TestScriptReader.sql not found");
    }
    
    [Fact]
    public async Task TestRead_When_ScriptFileEmpty()
    {
        var fileData = new MockFileData(string.Empty, Encoding.UTF8);
        _fileHelper.FileSystem.AddFile(TestFileName, fileData);
        
        var sqlStatements = await _scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Should().BeEmpty();
    }

    [Fact]
    public async Task TestReadSingleStatement_When_MissingEndOfStatement_Then_Ok()
    {
        var fileData = new MockFileData("first statement", Encoding.UTF8);
        _fileHelper.FileSystem.AddFile(TestFileName, fileData);
         
        var sqlStatements = await _scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Count.Should().Be(1);
    }

    [Fact]
    public async Task TestReadSeveralStatements_When_MissingEndOfLastStatement_Then_Ok()
    {
        var fileData = new MockFileData("statement1;" + Environment.NewLine + Environment.NewLine + "statement2;" + Environment.NewLine, Encoding.UTF8);
        _fileHelper.FileSystem.AddFile(TestFileName, fileData);
        
        var sqlStatements = await _scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Count.Should().Be(2);
    }

    [Fact]
    public async Task TestSingleNewLine_Then_SingleStatement()
    {
        var fileData = new MockFileData("statement1;" + Environment.NewLine + "more_statement1;", Encoding.UTF8);
        _fileHelper.FileSystem.AddFile(TestFileName, fileData);
        
        var sqlStatements = await _scriptsReader.ReadAsync(TestFileName).ToListAsync();
        
        sqlStatements.Count.Should().Be(1);
    }
}