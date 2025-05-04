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
}