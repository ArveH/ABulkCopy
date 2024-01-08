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

    public async Task<TableDefinition?> GetTableInfoAsync(string tableName)
    {
        var tableHeader = await _systemTables.GetTableHeaderAsync(tableName);
        if (tableHeader == null)
        {
            _logger.Warning("Table {TableName} not found", tableName);
            return null;
        }
        var columnInfo = await _systemTables.GetTableColumnInfoAsync(tableHeader);
        var primaryKey = await _systemTables.GetPrimaryKeyAsync(tableHeader);
        var foreignKeys = await _systemTables.GetForeignKeysAsync(tableHeader);
        var indexes = await _systemTables.GetIndexesAsync(tableHeader);

        return new TableDefinition(Rdbms.Mss)
        {
            Header = tableHeader,
            Columns = columnInfo.ToList(),
            PrimaryKey = primaryKey,
            ForeignKeys = foreignKeys.ToList(),
            Indexes = indexes.ToList()
        };
    }
}