namespace ABulkCopy.Common.Scripts;

public interface IScriptRunner
{
    Task<(int succeed, int fail)> ExecuteAsync(string scriptFilePath, CancellationToken ct);
}