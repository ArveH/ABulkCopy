namespace ABulkCopy.Common.Reader;

public interface IADataReader
{
    Task<long> Read(TableDefinition tableDefinition,
        string path);
}