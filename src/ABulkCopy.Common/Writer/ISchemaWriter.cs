namespace ABulkCopy.Common.Writer;

public interface ISchemaWriter
{
    Task WriteAsync(
        TableDefinition tableDefinition,
        string path);
}