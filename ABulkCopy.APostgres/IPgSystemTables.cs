namespace ABulkCopy.APostgres;

public interface IPgSystemTables
{
    Task<PrimaryKey?> GetPrimaryKeyAsync(TableHeader tableHeader);
    Task<IEnumerable<ForeignKey>> GetForeignKeysAsync(TableHeader tableHeader);
    Task<uint?> GetIdentityOidAsync(string tableName, string columnName, CancellationToken ct);
}