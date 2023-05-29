namespace ABulkCopy.Common.Reader;

public interface ISchemaReader
{
    Task<TableDefinition?> GetTableDefinition(string folderPath, string tableName);
}