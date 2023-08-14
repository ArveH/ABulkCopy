namespace ABulkCopy.Common.Reader;

public interface IDataFileReader
{
    void Open(string path);
    public string? ReadColumn(string colName, EmptyStringFlag emptyString = EmptyStringFlag.Leave);
    void ReadColumnSeparator(string colName);
    void ReadNewLine();
    public bool IsEndOfFile { get; }
    byte[] ReadAllBytes(string path);
    void SkipToNextLine();
    void Dispose();
}