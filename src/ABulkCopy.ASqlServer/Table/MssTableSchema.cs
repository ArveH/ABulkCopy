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

    public async Task<TableDefinition?> GetTableInfoAsync(
        string schemaName, string tableName, CancellationToken ct)
    {
        if (schemaName == string.Empty)
        {
            schemaName = "dbo";
        }

        var tableHeader = await _systemTables.GetTableHeaderAsync(
            schemaName, tableName, ct).ConfigureAwait(false);
        if (tableHeader == null)
        {
            _logger.Warning("Table {SchemaName}.{TableName} not found", 
                schemaName, tableName);
            return null;
        }
        var columnInfo = await _systemTables.GetTableColumnInfoAsync(tableHeader, ct).ConfigureAwait(false);
        var primaryKey = await _systemTables.GetPrimaryKeyAsync(tableHeader, ct).ConfigureAwait(false);
        var foreignKeys = await _systemTables.GetForeignKeysAsync(tableHeader, ct).ConfigureAwait(false);
        var indexes = await _systemTables.GetIndexesAsync(tableHeader, ct).ConfigureAwait(false);

        return new TableDefinition(Rdbms.Mss)
        {
            Header = tableHeader,
            Data = new TableData { FileName = $"{schemaName}.{tableName}{Constants.DataSuffix}"},
            Columns = columnInfo.ToList(),
            PrimaryKey = primaryKey,
            ForeignKeys = foreignKeys.ToList(),
            Indexes = indexes.ToList()
        };
    }
}