namespace ABulkCopy.ASqlServer;

public interface IMssCmd
{
    Task CreateTableAsync(
        TableDefinition tableDefinition,
        CancellationToken ct,
        bool addIfNotExists = false);
    Task ExecuteNonQueryAsync(
        string sqlString,
        CancellationToken ct);
    Task DropTableAsync(
        SchemaTableTuple st,
        CancellationToken ct);
    Task CreateIndexAsync(
        SchemaTableTuple st,
        IndexDefinition indexDefinition,
        CancellationToken ct);
    Task<object?> ExecuteScalarAsync(
        string sqlString,
        CancellationToken ct);

}