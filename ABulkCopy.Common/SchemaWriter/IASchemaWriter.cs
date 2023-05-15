namespace ABulkCopy.Common.SchemaWriter;

public interface IASchemaWriter
{
    Task Write(
        TableDefinition tableDefinition,
        string path);
}