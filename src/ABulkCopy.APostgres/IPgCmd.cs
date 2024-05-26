namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTableAsync(
        TableDefinition tableDefinition, 
        CancellationToken ct, 
        bool addIfNotExists = false);
    Task ExecuteNonQueryAsync(
        string sqlString,
        CancellationToken ct);
    Task DropTableAsync(SchemaTableTuple st, CancellationToken ct);
    Task CreateIndexAsync(
        string tableName, 
        IndexDefinition indexDefinition, 
        CancellationToken ct);
    Task<object?> SelectScalarAsync(
        string sqlString,
        CancellationToken ct);
}