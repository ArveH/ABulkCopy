namespace ABulkCopy.Common.Writer;

public interface ISchemaWriter
{
    Task Write(
        TableDefinition tableDefinition,
        string path);
}