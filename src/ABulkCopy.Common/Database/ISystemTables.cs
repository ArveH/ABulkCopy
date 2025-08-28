namespace ABulkCopy.Common.Database;

public interface ISystemTables
{
    Task<IEnumerable<SchemaTableTuple>> GetFullTableNamesAsync(
        string schemaNames, 
        string searchString, 
        CancellationToken ct);
    Task<TableHeader?> GetTableHeaderAsync(
        string schemaName, 
        string tableName, 
        CancellationToken ct);                                                               
    Task<IEnumerable<IColumn>> GetTableColumnInfoAsync(
        TableHeader tableHeader,            
        CancellationToken ct);                                                                            
    Task<PrimaryKey?> GetPrimaryKeyAsync(
        TableHeader tableHeader,                                         
        CancellationToken ct);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(
        TableHeader tableHeader, 
        CancellationToken ct);
}                                                                                                                                                                                                        