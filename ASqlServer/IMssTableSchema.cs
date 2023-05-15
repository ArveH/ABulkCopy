namespace ASqlServer;

public interface IMssTableSchema
{
    Task<TableDefinition?> GetTableInfo(string tableName);
}