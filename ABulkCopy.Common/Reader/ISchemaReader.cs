namespace ABulkCopy.Common.Reader;

public interface ISchemaReader
{
    Task<TableDefinition> GetTableDefinitionAsync(string folderPath, string tableName);
    Task<TableDefinition> GetTableDefinitionAsync(string fullPath);
}