namespace ABulkCopy.APostgres;

public interface IPgSystemTables
{
    Task<PrimaryKey?> GetPrimaryKeyAsync(TableHeader tableHeader, CancellationToken ct);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(TableHeader tableHeader, CancellationToken ct);
    Task<uint?> GetIdentityOidAsync(string tableName, string columnName, CancellationToken ct);
}