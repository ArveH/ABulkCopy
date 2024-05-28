namespace ABulkCopy.ASqlServer.Table;

public interface IMssTableSchema
{
    Task<TableDefinition?> GetTableInfoAsync(
        string schemaName, string tableName, CancellationToken ct);
}