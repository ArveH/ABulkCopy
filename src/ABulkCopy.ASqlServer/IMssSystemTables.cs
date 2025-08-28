namespace ABulkCopy.ASqlServer;

public interface IMssSystemTables: ISystemTables
{
    Task<IEnumerable<IndexDefinition>> GetIndexesAsync(TableHeader tableHeader, CancellationToken ct);
}