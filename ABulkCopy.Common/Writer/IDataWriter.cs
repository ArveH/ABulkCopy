namespace ABulkCopy.Common.Writer;

public interface IDataWriter
{
    Task WriteTable(
        ITableReader tableReader,
        TableDefinition tableDefinition,
        string path);
}