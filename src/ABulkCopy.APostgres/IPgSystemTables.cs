namespace ABulkCopy.APostgres;

public interface IPgSystemTables: ISystemTables
{
    Task ResetIdentityAsync(
        string tableName,
        string columnName,
        CancellationToken ct);
}