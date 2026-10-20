namespace ABulkCopy.Common;

public interface ITableSchema
{
    Task<TableDefinition?> GetTableInfoAsync(
        SchemaTableTuple st, CancellationToken ct);
}