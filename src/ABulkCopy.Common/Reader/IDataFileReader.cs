namespace ABulkCopy.Common.Reader;

public interface IDataFileReader
{
    void Open(string path, InsertSettings insertSettings);
    public string? ReadColumn(string colName);
    void ReadColumnSeparator(string colName);
    void ReadNewLine();
    public bool IsEndOfFile { get; }
    byte[] ReadAllBytes(string path);
    void SkipToNextLine();
    void Dispose();
}