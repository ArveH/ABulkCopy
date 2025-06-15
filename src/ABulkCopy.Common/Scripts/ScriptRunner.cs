namespace ABulkCopy.Common.Scripts;

public class ScriptRunner
{
    private readonly IScriptReader _scriptReader;
    private readonly IDbRawCommand _rawCommand;

    public ScriptRunner(
        IScriptReader scriptReader,
        IDbRawCommand rawCommand)
    {
        _scriptReader = scriptReader;
        _rawCommand = rawCommand;
    }

    public async Task ExecuteAsync(string scriptFilePath, CancellationToken ct)
    {
        await foreach (var stmt in _scriptReader.ReadAsync(scriptFilePath, ct))
        {
            await _rawCommand.ExecuteNonQueryAsync(stmt, ct);
        }
    }
}