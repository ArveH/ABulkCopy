namespace ABulkCopy.Common.Writer;

public interface IDataWriter
{
    Task WriteTable(
        TableDefinition tableDefinition,
        string path);
}