namespace ABulkCopy.Common.Reader;

public interface ISchemaReader
{
    Task<TableDefinition> GetTableDefinitionAsync(
        string folderPath, SchemaTableTuple st, CancellationToken ct);
    Task<TableDefinition> GetTableDefinitionAsync(
        string fullPath, CancellationToken ct);
}