namespace ABulkCopy.APostgres;

public interface IPgCmd
{
    Task CreateTableAsync(
        TableDefinition tableDefinition, 
        CancellationToken ct, 
        bool addIfNotExists = false);
    Task DropTableAsync(
        SchemaTableTuple st, 
        CancellationToken ct);
    Task CreateIndexAsync(
        SchemaTableTuple st, 
        IndexDefinition indexDefinition, 
        CancellationToken ct);
}