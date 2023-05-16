namespace ASqlServer;

public interface IMssDataReader
{
    string ConnectionString { get; init; }
    Task PrepareReader(TableDefinition tableDefinition);
    Task<bool> Read();
    void Close();
    void Dispose();
    bool IsNull(int ordinal);
    object? GetValue(int ordinal);
}