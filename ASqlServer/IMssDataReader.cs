namespace ASqlServer;

public interface IMssDataReader
{
    string ConnectionString { get; init; }
    Task PrepareReader(TableDefinition tableDefinition);
    Task<bool> Read();
    void Close();
    void Dispose();
}