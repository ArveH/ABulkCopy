namespace ABulkCopy.Common.Scripts;

public class ScriptReader
{
    private readonly IFileSystem _fileSystem;

    public ScriptReader(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async IAsyncEnumerable<string> ReadAsync(string scriptFile)
    {
        if (!_fileSystem.File.Exists(scriptFile))
        {
            throw new FileNotFoundException($"File {scriptFile} not found");
        }
       
        await Task.CompletedTask;
        yield break;
    }
}