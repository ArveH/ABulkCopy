namespace ABulkCopy.APostgres;

public interface IPgSystemTables: ISystemTables
{
    Task ResetIdentityAsync(
        SchemaTableTuple st,
        string columnName,
        CancellationToken ct);
}