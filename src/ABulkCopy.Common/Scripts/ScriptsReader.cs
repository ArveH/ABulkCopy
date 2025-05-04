namespace ABulkCopy.Common.Scripts;

public class ScriptsReader
{
    private readonly IFileSystem _fileSystem;

    public ScriptsReader(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async IAsyncEnumerable<string> GetScriptsAsync(string scriptFile)
    {
        if (!File.Exists(scriptFile))
        {
            throw new FileNotFoundException($"File {scriptFile} not found");
        }
       
        await Task.CompletedTask;
        yield break;
    }
}