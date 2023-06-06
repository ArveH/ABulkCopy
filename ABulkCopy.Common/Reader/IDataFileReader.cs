namespace ABulkCopy.Common.Reader;

public interface IDataFileReader
{
    public string? ReadColumn(string colName);
    void ReadColumnSeparator(string colName);
}