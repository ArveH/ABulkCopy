namespace ABulkCopy.ASqlServer;

public interface IMssTableReader : ITableReader
{
    string ConnectionString { get; init; }
    void Dispose();
}