namespace ABulkCopy.APostgres;

public interface IPgTableReader : ITableReader
{
    string ConnectionString { get; init; }
    void Dispose();
}
