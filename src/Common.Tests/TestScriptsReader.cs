namespace Common.Tests;

public class TestScriptsReader
{
    [Fact]
    public async Task TestRead_When_ScriptsFileNotFound()
    {
        var fileHelper = new FileHelper();
        var scriptsReader = new ScriptsReader(fileHelper.FileSystem);
        
        var act = async () => await scriptsReader.GetScriptsAsync("TestScriptsReader.sql").ToListAsync();
        
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("File TestScriptsReader.sql not found");
    }
}