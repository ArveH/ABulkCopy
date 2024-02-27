namespace ABulkCopy.Common.Writer;

public interface IDataWriter
{
    Task<long> WriteAsync(TableDefinition tableDefinition,
        string path, CancellationToken ct);
}