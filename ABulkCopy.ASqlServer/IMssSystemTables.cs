namespace ABulkCopy.ASqlServer;

public interface IMssSystemTables
{
    Task<IEnumerable<string>> GetTableNamesAsync(string searchString, CancellationToken ct);
    Task<TableHeader?> GetTableHeaderAsync(string tableName, CancellationToken ct);
    Task<IEnumerable<IColumn>> GetTableColumnInfoAsync(TableHeader tableHeader, CancellationToken ct);
    Task<PrimaryKey?> GetPrimaryKeyAsync(TableHeader tableHeader, CancellationToken ct);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(TableHeader tableHeader, CancellationToken ct);
    Task<IEnumerable<IndexDefinition>> GetIndexesAsync(TableHeader tableHeader, CancellationToken ct);
}