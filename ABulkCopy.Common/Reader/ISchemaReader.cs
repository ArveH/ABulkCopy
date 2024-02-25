namespace ABulkCopy.Common.Reader;

public interface ISchemaReader
{
    Task<TableDefinition> GetTableDefinitionAsync(
        string folderPath, string tableName, CancellationToken ct);
    Task<TableDefinition> GetTableDefinitionAsync(
        string fullPath, CancellationToken ct);
}