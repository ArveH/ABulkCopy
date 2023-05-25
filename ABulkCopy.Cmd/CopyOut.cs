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
        _logger.Information("Copy direction: Out");

        var tableNames = await _systemTables.GetTableNames(searchStr);
        foreach (var tableName in tableNames)
        {
            try
            {
                var tabDef = await _tableSchema.GetTableInfo(tableName);
                if (tabDef == null)
                {
                    _logger.Warning("Table {SearchString} not found", tableName);
                    continue;
                }

                await _schemaWriter.Write(tabDef, folder);
                var rows = await _dataWriter.Write(tabDef, folder);
                _logger.Information("Table '{TableName}' with {Rows} rows copied to disk",
                    tableName, rows);
                Console.WriteLine($"Table '{tableName}' with {rows} rows copied to disk");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Copy out from table '{TableName}' failed",
                    tableName);
                Console.WriteLine($"Copy out from table '{tableName}' failed");
            }
        }
    }
}