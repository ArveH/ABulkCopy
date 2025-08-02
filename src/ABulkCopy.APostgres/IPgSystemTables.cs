namespace ABulkCopy.APostgres;

public interface IPgSystemTables: ISystemTables
{
    Task<TableHeader?> GetTableHeaderAsync(
        string schemaName, 
        string tableName, 
        CancellationToken ct);
    Task<PrimaryKey?> GetPrimaryKeyAsync(
        TableHeader tableHeader, 
        CancellationToken ct);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(
        TableHeader tableHeader, 
        CancellationToken ct);
    Task<uint?> GetIdentityOidAsync(
        string tableName, 
        string columnName, 
        CancellationToken ct);
    Task ResetIdentityAsync(
        string tableName,
        string columnName,
        CancellationToken ct);
}