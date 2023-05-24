namespace ABulkCopy.Common.Writer;

public interface IDataWriter
{
    Task<long> Write(TableDefinition tableDefinition,
        string path);
}