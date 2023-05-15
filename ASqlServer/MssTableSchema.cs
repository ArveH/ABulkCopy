namespace ASqlServer;

public class MssTableSchema : IMssTableSchema
{
    private readonly IASqlCommand _command;
    private readonly ILogger _logger;

    public MssTableSchema(
        IASqlCommand command,
        ILogger logger)
    {
        _command = command;
        _logger = logger.ForContext<MssTableSchema>();
    }

    public async Task<TableDefinition?> GetTableInfo(string tableName)
    {
        var tableHeader = await _command.GetTableHeader(tableName);
        if (tableHeader == null)
        {
            _logger.Warning("Table {TableName} not found", tableName);
            return null;
        }
        var columnInfo = await _command.GetColumnInfo(tableHeader);
        var primaryKey = await _command.GetPrimaryKey(tableHeader);
        var foreignKeys = await _command.GetForeignKeys(tableHeader);

        return new TableDefinition
        {
            Header = tableHeader,
            Columns = columnInfo.ToList(),
            PrimaryKey = primaryKey,
            ForeignKeys = foreignKeys.ToList()
        };
    }
}