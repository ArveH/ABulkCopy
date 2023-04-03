namespace ASqlServer;

public interface IASqlCommand
{
    string ConnectionString { get; init; }
    IAsyncEnumerable<string> GetTableNames(string searchString);
    Task<TableDefinition?> GetTableInfo(string tableName);
}