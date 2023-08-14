namespace ABulkCopy.Common.Reader;

public interface IADataReader
{
    Task<long> Read(string folder, TableDefinition tableDefinition, EmptyStringFlag emptyStringFlag = EmptyStringFlag.Leave);
    void Dispose();
}