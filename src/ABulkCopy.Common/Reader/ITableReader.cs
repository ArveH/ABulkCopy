namespace ABulkCopy.Common.Reader;

public interface ITableReader
{
    Task PrepareReaderAsync(TableDefinition tableDefinition, CancellationToken ct);
    Task<bool> ReadAsync(CancellationToken ct);
    bool IsNull(int ordinal);
    object? GetValue(int ordinal);
    long GetBytes(int ordinal, long startIndex, byte[] buf, int length);
    void Close();
}