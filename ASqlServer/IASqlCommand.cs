namespace ASqlServer;

public interface IASqlCommand
{
    string ConnectionString { get; init; }
    Task<IEnumerable<string>> GetTableNames(string searchString);
    Task<TableDefinition?> GetTableInfo(string tableName);
}