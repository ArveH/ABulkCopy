namespace ABulkCopy.Common.Reader;

public interface ITableReader
{
    Task PrepareReader(TableDefinition tableDefinition);
    Task<bool> Read();
    bool IsNull(int ordinal);
    object? GetValue(int ordinal);
    void Close();
}