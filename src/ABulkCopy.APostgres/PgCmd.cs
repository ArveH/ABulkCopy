namespace ABulkCopy.APostgres;

public class PgCmd : IPgCmd
{
    private readonly IDbRawCommand _dbRawCommand;
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public PgCmd(
        IDbRawCommand dbRawCommand,
        IQueryBuilderFactory queryBuilderFactory)
    {
        _dbRawCommand = dbRawCommand;
        _queryBuilderFactory = queryBuilderFactory;
    }


    public async Task DropTableAsync(SchemaTableTuple st, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _dbRawCommand.ExecuteNonQueryAsync(
            qb.DropTableStmt(st), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(
        TableDefinition tableDefinition, 
        CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _dbRawCommand.ExecuteNonQueryAsync(
            qb.CreateTableStmt(tableDefinition, addIfNotExists), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(
        SchemaTableTuple st, IndexDefinition indexDefinition, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await _dbRawCommand.ExecuteNonQueryAsync(
            qb.CreateIndexStmt(st, indexDefinition), 
            ct).ConfigureAwait(false);
    }
}