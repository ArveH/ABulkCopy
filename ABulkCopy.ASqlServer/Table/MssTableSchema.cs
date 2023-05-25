namespace ABulkCopy.ASqlServer.Table;

public class MssTableSchema : IMssTableSchema
{
    private readonly IMssSystemTables _systemTables;
    private readonly ILogger _logger;

    public MssTableSchema(
        IMssSystemTables systemTables,
        ILogger logger)
    {
        _systemTables = systemTables;
        _logger = logger.ForContext<MssTableSchema>();
    }

    public async Task<TableDefinition?> GetTableInfo(string tableName)
    {
        var tableHeader = await _systemTables.GetTableHeader(tableName);
        if (tableHeader == null)
        {
            _logger.Warning("Table {TableName} not found", tableName);
            return null;
        }
        var columnInfo = await _systemTables.GetColumnInfo(tableHeader);
        var primaryKey = await _systemTables.GetPrimaryKey(tableHeader);
        var foreignKeys = await _systemTables.GetForeignKeys(tableHeader);

        return new TableDefinition
        {
            Header = tableHeader,
            Columns = columnInfo.ToList(),
            PrimaryKey = primaryKey,
            ForeignKeys = foreignKeys.ToList()
        };
    }
}