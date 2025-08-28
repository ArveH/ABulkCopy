namespace ABulkCopy.Common;

public interface ITableSchema
{
    Task<TableDefinition?> GetTableInfoAsync(
        string schemaName, string tableName, CancellationToken ct);
}