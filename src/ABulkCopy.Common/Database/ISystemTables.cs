namespace ABulkCopy.Common.Database;

public interface ISystemTables
{
    Task<IEnumerable<SchemaTableTuple>>
        GetFullTableNamesAsync(string schemaNames, string searchString, CancellationToken ct);
}