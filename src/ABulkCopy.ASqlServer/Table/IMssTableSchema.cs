namespace ABulkCopy.ASqlServer.Table;

public interface IMssTableSchema
{
    Task<TableDefinition?> GetTableInfoAsync(
        string tableName, CancellationToken ct);
}