namespace ABulkCopy.Common.Reader;

public interface IADataReader
{
    Task<long> ReadAsync(
        string folder, 
        TableDefinition tableDefinition,
        CancellationToken ct,
        EmptyStringFlag emptyStringFlag = EmptyStringFlag.Leave);
    void Dispose();
}