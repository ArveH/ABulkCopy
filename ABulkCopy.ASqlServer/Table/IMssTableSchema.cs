namespace ABulkCopy.ASqlServer.Table;

public interface IMssTableSchema
{
    Task<TableDefinition?> GetTableInfo(string tableName);
}