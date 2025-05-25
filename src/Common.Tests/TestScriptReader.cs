using ABulkCopy.Common.Exceptions;

namespace Common.Tests;

public class TestScriptReader
{
    private const string TestFileName = "TestScriptReader.sql";
    
    [Fact]
    public async Task TestRead_When_ScriptFileNotFound()
    {
        var fileHelper = new FileHelper();
        var scriptsReader = new ScriptReader(fileHelper.FileSystem);
        
        var act = async () => await scriptsReader.ReadAsync(TestFileName).ToListAsync();
        
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("File TestScriptReader.sql not found");
    }
    
    [Fact]
    public async Task TestRead_When_ScriptFileEmpty()
    {
        var fileHelper = new FileHelper();
        var fileData = new MockFileData("", Encoding.UTF8);
        fileHelper.FileSystem.AddFile("TestScriptReader.sql", fileData);
        var scriptsReader = new ScriptReader(fileHelper.FileSystem);
        
        var sqlStatements = await scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Should().BeEmpty();
    }

    [Fact]
    public async Task TestReadSingleStatement_When_MissingEndOfStatement_Then_Ok()
    {
        var fileHelper = new FileHelper();
        var fileData = new MockFileData("first statement", Encoding.UTF8);
        fileHelper.FileSystem.AddFile("TestScriptReader.sql", fileData);
        var scriptsReader = new ScriptReader(fileHelper.FileSystem);
        
        var sqlStatements = await scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Count.Should().Be(1);
    }

    [Fact]
    public async Task TestReadSeveralLines_When_MissingEndOfStatement_Then_Ok()
    {
        var fileHelper = new FileHelper();
        var fileData = new MockFileData("statement1;" + Environment.NewLine + Environment.NewLine + "statement2;" + Environment.NewLine, Encoding.UTF8);
        fileHelper.FileSystem.AddFile("TestScriptReader.sql", fileData);
        var scriptsReader = new ScriptReader(fileHelper.FileSystem);
        
        var sqlStatements = await scriptsReader.ReadAsync(TestFileName).ToListAsync();

        sqlStatements.Count.Should().Be(2);
    }

    [Fact]
    public async Task TestSingleNewLine_Then_SingleStatement()
    {
        var fileHelper = new FileHelper();
        var fileData = new MockFileData("statement1;" + Environment.NewLine + "statement2;", Encoding.UTF8);
        fileHelper.FileSystem.AddFile("TestScriptReader.sql", fileData);
        var scriptsReader = new ScriptReader(fileHelper.FileSystem);
        
        var sqlStatements = await scriptsReader.ReadAsync(TestFileName).ToListAsync();
        
        sqlStatements.Count.Should().Be(1);
    }
}