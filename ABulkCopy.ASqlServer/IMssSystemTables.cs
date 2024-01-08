namespace ABulkCopy.ASqlServer;

public interface IMssSystemTables
{
    Task<IEnumerable<string>> GetTableNamesAsync(string searchString);
    Task<TableHeader?> GetTableHeaderAsync(string tableName);
    Task<IEnumerable<IColumn>> GetTableColumnInfoAsync(TableHeader tableHeader);
    Task<PrimaryKey?> GetPrimaryKeyAsync(TableHeader tableHeader);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(TableHeader tableHeader);
    Task<IEnumerable<IndexDefinition>> GetIndexesAsync(TableHeader tableHeader);
}