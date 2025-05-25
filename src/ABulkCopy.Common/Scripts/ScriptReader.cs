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

        var sb = new StringBuilder();

        var file = _fileSystem.File.OpenText(scriptFile);
        while (!file.EndOfStream)
        {
            var stmt = await ReadStatementAsync(file, sb);
            if (string.IsNullOrEmpty(stmt))
            {
                yield break;
            }

            yield return stmt;
        }
    }

    private static async Task<string?> ReadStatementAsync(
        StreamReader file, 
        StringBuilder sb)
    {
        sb.Clear();
        if (file.EndOfStream)
        {
            return sb.ToString();
        }
        
        var line = await file.ReadLineAsync();
        while (!string.IsNullOrEmpty(line))
        {
            sb.AppendLine(line); 
            if (file.EndOfStream)
            {
                return sb.ToString();
            }
            line = await file.ReadLineAsync();
        }

        return sb.ToString();
    }
}