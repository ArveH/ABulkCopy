namespace ABulkCopy.Common.Scripts;

public interface IScriptReader
{
    IAsyncEnumerable<string> ReadAsync(string scriptFile, CancellationToken ct);
}