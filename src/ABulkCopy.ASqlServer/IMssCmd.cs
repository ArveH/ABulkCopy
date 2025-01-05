namespace ABulkCopy.ASqlServer;

public interface IMssCmd
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

    Task EnsureSchemaAsync(string name);

    Task DropIndexAsync(
        SchemaTableTuple st, 
        string indexName,
        CancellationToken ct);
}