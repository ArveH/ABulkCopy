namespace ABulkCopy.APostgres;

public class PgCmd : IPgCmd
{
    private readonly IPgRawCommand _pgRawCommand;
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public PgCmd(
        IPgRawCommand pgRawCommand,
        IQueryBuilderFactory queryBuilderFactory)
    {
        _pgRawCommand = pgRawCommand;
        _queryBuilderFactory = queryBuilderFactory;
    }

    public async Task DropTableAsync(SchemaTableTuple st, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _pgRawCommand.ExecuteNonQueryAsync(
            qb.DropTableStmt(st), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(
        
        
        TableDefinition tableDefinition, 
        CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _pgRawCommand.ExecuteNonQueryAsync(
            qb.CreateTableStmt(tableDefinition, addIfNotExists), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(
        SchemaTableTuple st, IndexDefinition indexDefinition, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _pgRawCommand.ExecuteNonQueryAsync(
            qb.CreateIndexStmt(st, indexDefinition), 
            ct).ConfigureAwait(false);
    }
    
    public async Task EnsureSchemaAsync(string name)
    {
        await _pgRawCommand.ExecuteNonQueryAsync($"CREATE SCHEMA IF NOT EXISTS {name}", CancellationToken.None);
    }
}