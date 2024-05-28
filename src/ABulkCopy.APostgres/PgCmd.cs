namespace ABulkCopy.APostgres;

public class PgCmd : PgCommandBase, IPgCmd
{
    private readonly IQueryBuilderFactory _queryBuilderFactory;

    public PgCmd(
        IPgContext pgContext,
        IQueryBuilderFactory queryBuilderFactory) : base(pgContext)
    {
        _queryBuilderFactory = queryBuilderFactory;
    }

    public async Task DropTableAsync(SchemaTableTuple st, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(
            qb.DropTableStmt(st), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateTableAsync(
        TableDefinition tableDefinition, 
        CancellationToken ct,
        bool addIfNotExists = false)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(
            qb.CreateTableStmt(tableDefinition, addIfNotExists), 
            ct).ConfigureAwait(false);
    }

    public async Task CreateIndexAsync(
        SchemaTableTuple st, IndexDefinition indexDefinition, CancellationToken ct)
    {
        var qb = _queryBuilderFactory.GetQueryBuilder();
        await ExecuteNonQueryAsync(
            qb.CreateIndexStmt(st, indexDefinition), 
            ct).ConfigureAwait(false);
    }

}