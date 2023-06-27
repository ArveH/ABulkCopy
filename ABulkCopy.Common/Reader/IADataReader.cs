namespace ABulkCopy.Common.Reader;

public interface IADataReader
{
    Task<long> Read(string folder, TableDefinition tableDefinition);
    void Dispose();
}