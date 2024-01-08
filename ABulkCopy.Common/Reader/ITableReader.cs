namespace ABulkCopy.Common.Reader;

public interface ITableReader
{
    Task PrepareReaderAsync(TableDefinition tableDefinition);
    Task<bool> ReadAsync();
    bool IsNull(int ordinal);
    object? GetValue(int ordinal);
    long GetBytes(int ordinal, long startIndex, byte[] buf, int length);
    void Close();
}