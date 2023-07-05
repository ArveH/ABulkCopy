namespace ABulkCopy.Cmd;

public class CopyOut : ICopyOut
{
    private readonly IMssSystemTables _systemTables;
    private readonly IMssTableSchema _tableSchema;
    private readonly IDataWriter _dataWriter;
    private readonly ISchemaWriter _schemaWriter;
    private readonly ILogger _logger;

    public CopyOut(
        IMssSystemTables systemTables,
        IMssTableSchema tableSchema,
        IDataWriter dataWriter,
        ISchemaWriter schemaWriter,
        ILogger logger)
    {
        _systemTables = systemTables;
        _tableSchema = tableSchema;
        _dataWriter = dataWriter;
        _schemaWriter = schemaWriter;
        _logger = logger.ForContext<CopyOut>();
    }

    public async Task Run(string folder, string searchStr)
    {
        var sw = new Stopwatch();
        sw.Start();
        var tableNames = (await _systemTables.GetTableNames(searchStr)).ToList();
        _logger.Information($"Copy out {{TableCount}} {"table".Plural(tableNames.Count)}",
            tableNames.Count);
        Console.WriteLine($"Copy out {tableNames.Count} {"table".Plural(tableNames.Count)}.");
        var errors = 0;
        await Parallel.ForEachAsync(tableNames, async (tableName, _) =>
        {
            if (!await CopyTable(folder, tableName))
            {
                Interlocked.Increment(ref errors);
            }
        });
        sw.Stop();

        if (errors > 0)
        {
            _logger.Warning($"Copy out {{TableCount}} {"table".Plural(tableNames.Count)} finished with {{Errors}} {"error".Plural(errors)}", 
                tableNames.Count, errors);
            Console.WriteLine($"Copy out {tableNames.Count} {"table".Plural(tableNames.Count)} finished with {errors} {"error".Plural(errors)}");
        }
        else
        {
            _logger.Information($"Copy out {{TableCount}} {"table".Plural(tableNames.Count)} finished.", 
                tableNames.Count);
            Console.WriteLine($"Copy out {tableNames.Count} {"table".Plural(tableNames.Count)} finished.");
        }
        _logger.Information("Copy took {Elapsed}", sw.Elapsed.ToString("g"));
        Console.WriteLine($"Copy took {sw.Elapsed:g}");
    }

    private async Task<bool> CopyTable(string folder, string tableName)
    {
        try
        {
            var tabDef = await _tableSchema.GetTableInfo(tableName);
            if (tabDef == null)
            {
                _logger.Warning("Table {SearchString} not found", tableName);
                return false;
            }

            await _schemaWriter.Write(tabDef, folder);
            var rows = await _dataWriter.Write(tabDef, folder);
            _logger.Information("Table '{TableName}' with {Rows} rows copied to disk",
                tableName, rows);
            Console.WriteLine($"Table '{tableName}' with {rows} rows copied to disk");
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Copy out from table '{TableName}' failed",
                tableName);
            Console.WriteLine($"Copy out from table '{tableName}' failed");
            return false;
        }
    }
}