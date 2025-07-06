namespace ABulkCopy.Common.Scripts;

public class ScriptRunner
{
    private readonly IScriptReader _scriptReader;
    private readonly IDbRawCommand _rawCommand;
    private readonly ILogger _logger;

    public ScriptRunner(
        IScriptReader scriptReader,
        IDbRawCommand rawCommand,
        ILogger logger)
    {
        _scriptReader = scriptReader;
        _rawCommand = rawCommand;
        _logger = logger;
    }

    public async Task<(int succeed, int fail)> ExecuteAsync(string scriptFilePath, CancellationToken ct)
    {
        var succeedCounter = 0;
        var errorCounter = 0;
        _logger.Information("Executing script: {ScriptFilePath}", scriptFilePath);
        await foreach (var stmt in _scriptReader.ReadAsync(scriptFilePath, ct))
        {
            try
            {
                await _rawCommand.ExecuteNonQueryAsync(stmt, ct);
                succeedCounter++;
            }
            catch (Exception ex)
            {
                errorCounter++;
                _logger.Warning(ex.Message);
            }
        }
        
        _logger.Information("Done executing script: {ScriptFilePath}. {CmdCounter} commands executed.", 
           scriptFilePath, succeedCounter+errorCounter);
        if (errorCounter > 0)
        {
            _logger.Error("{ErrorCounter} command(s) filed", errorCounter);
        }
        
        return (succeedCounter,  errorCounter);
    }
}