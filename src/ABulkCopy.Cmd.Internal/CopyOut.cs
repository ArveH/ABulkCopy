namespace ABulkCopy.Cmd.Internal;

public class CopyOut : ICopyOut
{
    private readonly IConfiguration _config;
    private readonly IMssSystemTables _systemTables;
    private readonly IMssTableSchema _tableSchema;
    private readonly IDataWriter _dataWriter;
    private readonly ISchemaWriter _schemaWriter;
    private readonly ILogger _logger;

    public CopyOut(
        IConfiguration config,
        IMssSystemTables systemTables,
        IMssTableSchema tableSchema,
        IDataWriter dataWriter,
        ISchemaWriter schemaWriter,
        ILogger logger)
    {
        _config = config;
        _systemTables = systemTables;
        _tableSchema = tableSchema;
        _dataWriter = dataWriter;
        _schemaWriter = schemaWriter;
        _logger = logger.ForContext<CopyOut>();
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var sw = new Stopwatch();
        sw.Start();
        var fullNames = (await _systemTables.GetFullTableNamesAsync(
            _config.SafeGet(Constants.Config.SchemaFilter), 
            _config.SafeGet(Constants.Config.SearchFilter), 
            ct).ConfigureAwait(false)).ToList();
        _logger.Information($"Copy out {{TableCount}} {"table".Plural(fullNames.Count)}...",
            fullNames.Count);
        Console.WriteLine($"Copy out {fullNames.Count} {"table".Plural(fullNames.Count)}...");
        var errors = 0;
        await Parallel.ForEachAsync(fullNames, ct, async (fullName, _) =>
        {
            if (!await CopyTableAsync(
                    _config.Check(Constants.Config.Folder), 
                    fullName.schemaName,
                    fullName.tableName, 
                    ct).ConfigureAwait(false))
            {
                Interlocked.Increment(ref errors);
            }
        }).ConfigureAwait(false);
        sw.Stop();

        if (errors > 0)
        {
            _logger.Warning($"Copy out {{TableCount}} {"table".Plural(fullNames.Count)} finished with {{Errors}} {"error".Plural(errors)}", 
                fullNames.Count, errors);
            Console.WriteLine($"Copy out {fullNames.Count} {"table".Plural(fullNames.Count)} finished with {errors} {"error".Plural(errors)}");
        }
        else
        {
            _logger.Information($"Copy out {{TableCount}} {"table".Plural(fullNames.Count)} finished.", 
                fullNames.Count);
            Console.WriteLine($"Copy out {fullNames.Count} {"table".Plural(fullNames.Count)} finished.");
        }
        _logger.Information("Copy took {Elapsed}", sw.Elapsed.ToString("g"));
        Console.WriteLine($"Copy took {sw.Elapsed:g}");
    }

    private async Task<bool> CopyTableAsync(
        string folder, 
        string schemaName,
        string tableName, 
        CancellationToken ct)
    {
        try
        {
            // TODO: CancellationToken
            var tabDef = await _tableSchema.GetTableInfoAsync(schemaName, tableName, ct)
                .ConfigureAwait(false);
            if (tabDef == null)
            {
                _logger.Warning("Table {SearchString} not found", tableName);
                return false;
            }

            // TODO: CancellationToken
            await _schemaWriter.WriteAsync(tabDef, folder).ConfigureAwait(false);
            var rows = await _dataWriter.WriteAsync(tabDef, folder, ct).ConfigureAwait(false);
            _logger.Information("Table '{SchemaName}.{TableName}' with {Rows} rows copied to disk",
                schemaName, tableName, rows);
            Console.WriteLine($"Table '{schemaName}.{tableName}' with {rows} rows copied to disk");
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Copy out from table '{SchemaName}.{TableName}' failed",
                schemaName, tableName);
            Console.WriteLine($"Copy out from table '{schemaName}.{tableName}' failed");
            return false;
        }
    }
}